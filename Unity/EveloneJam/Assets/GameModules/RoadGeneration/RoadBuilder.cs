using ARTEX.Procedural.Racing;
using Project.Interaction;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.RoadGeneration
{
    [RequireComponent(typeof(MeshCollider))]
    public class RoadBuilder : MonoBehaviour, IRoadProvider
    {
        private const string ROAD_TAG = "Road";

        // Serialized Fields
        [SerializeField] public int Seed = -1;
        [SerializeField] private PathGenerator _generator;
        [SerializeField] private Road[] _prefabs;
        [SerializeField] private bool _combineRoadColliders = true;
        [SerializeField] private float _gridChunkSize = 8f;
        [SerializeField] private int _coinAmount = 2;
        [SerializeField] private Coin _pointPrefab;
        [SerializeField] private int _itemboxAmount = 2;
        [SerializeField] private ItemBox _itemBoxPrefab;

        // Private Variables
        private DiContainer _container;
        private MeshCollider _meshCollider;
        private List<Road> _starts = new List<Road>();
        private List<Road> _straights = new List<Road>();
        private List<Road> _leftTurns = new List<Road>();
        private List<Road> _rightTurns = new List<Road>();
        private List<Vector3Int> _path = new List<Vector3Int>();
        private List<Road> _spawnedRoads = new List<Road>();
        private List<RoadCheckpoint> _checkpoints = new List<RoadCheckpoint>();
        private List<Transform> _kartSpawnpoints = new List<Transform>();
        private bool[,] _roadMask;
        private int _startRoadIndex = -1;
        private float _rotation = 0;

        // Properties
        public IReadOnlyList<Road> SpawnedRoads => _spawnedRoads;
        public IReadOnlyList<RoadCheckpoint> Checkpoints => _checkpoints;
        public IReadOnlyList<Transform> KartSpawnpoints => _kartSpawnpoints;
        public float Width => _generator.Width * _gridChunkSize;
        public float Length => _generator.Length * _gridChunkSize;
        public float ChunkSize => _gridChunkSize;

        public event Action OnRoadCreated;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        private void Awake()
        {
            _meshCollider = GetComponent<MeshCollider>();
            FindRoadPrefabs();
        }

        private void FindRoadPrefabs()
        {
            _starts = FindRoadPrefabsByType(Road.RoadType.StraightStart, "No starts found");
            _straights = FindRoadPrefabsByType(Road.RoadType.Straight, "No straight roads found");
            _leftTurns = FindRoadPrefabsByType(Road.RoadType.LeftTurn, "No left turns found");
            _rightTurns = FindRoadPrefabsByType(Road.RoadType.RightTurn, "No right turns found");
        }

        private List<Road> FindRoadPrefabsByType(Road.RoadType roadType, string errorMessage)
        {
            List<Road> roads = _prefabs.Where(x => x.Type == roadType).ToList();
            if (roads.Count == 0) throw new Exception(errorMessage);
            return roads;
        }

        public void CreateNewRoad()
        {
            if (Seed == -1)
            {
                Seed = Random.Range(0, 1342435);
            }
            Random.InitState(Seed);

            DestroySpawnedRoads();
            _path = _generator.GeneratePath();
            SpawnMeshes();

            OnRoadCreated?.Invoke();
        }

        public bool HasRoadAtPosition(Vector3 position)
        {
            position /= _gridChunkSize;

            int x = Mathf.RoundToInt(position.x);
            int z = Mathf.RoundToInt(position.z);

            if (x < 0 || x >= _generator.Width || z < 0 || z >= _generator.Length) return false;

            return _roadMask[x, z];
        }

        private void DestroySpawnedRoads()
        {
            foreach (var spawnedRoad in _spawnedRoads)
            {
                Destroy(spawnedRoad.gameObject);
            }

            _spawnedRoads.Clear();
            _checkpoints.Clear();
            _kartSpawnpoints.Clear();
            _startRoadIndex = -1;

            _roadMask = new bool[_generator.Width, _generator.Length];
        }

        private void SpawnMeshes()
        {
            Vector3Int lastDirection = _path[0] - _path[_path.Count - 2];
            _rotation = Vector3.SignedAngle(Vector3.forward, lastDirection, Vector3.up);

            for (int i = 0; i < _path.Count - 1; i++)
            {
                Vector3Int currentDirection = _path[(i + 1) % _path.Count] - _path[i];
                Vector3 currentPosition = (Vector3)_path[i] * _gridChunkSize;
                float angle = Vector3.SignedAngle(lastDirection, currentDirection, Vector3.up);

                Road prefab = SelectRoadPrefab(angle, i);
                Road road = InstantiateRoad(prefab, currentPosition);

                _spawnedRoads.Add(road);
                

                lastDirection = currentDirection;
                _rotation += angle;

                _roadMask[_path[i].x, _path[i].z] = true;
            }

            if (_startRoadIndex == -1) throw new Exception("No start created");

            _kartSpawnpoints.AddRange(_spawnedRoads[_startRoadIndex].KartSpawnpoints);

            List<Road> newRoads = new List<Road>(_spawnedRoads.Count);
            for (int i = 0; i < _spawnedRoads.Count; i++)
            {
                newRoads.Add(_spawnedRoads[(i + _startRoadIndex) % _spawnedRoads.Count]);
            }
            _spawnedRoads = newRoads;

            GeneratePickups();

            for (int i = 0; i < _spawnedRoads.Count; i++)
            {
                _checkpoints.AddRange(_spawnedRoads[i].Checkpoints);
            }

            for (int i = 0; i < _checkpoints.Count; i++)
            {
                _checkpoints[i].Init(i);
            }

            _meshCollider.enabled = false;
            _meshCollider.sharedMesh = null;

            if (_combineRoadColliders)
            {
                CombineColliders();
            }
        }

        private Road SelectRoadPrefab(float angle, int index)
        {
            Road prefab = null;

            switch (angle)
            {
                case 0:
                    Debug.Log("Selecting straight road.");
                    if (_startRoadIndex == -1)
                    {
                        prefab = _starts[Random.Range(0, _starts.Count)];
                        _startRoadIndex = index;
                    }
                    else
                    {
                        prefab = _straights[Random.Range(0, _straights.Count)];
                    }
                    break;
                case 90:
                    Debug.Log("Selecting right turn road.");
                    prefab = _rightTurns[Random.Range(0, _rightTurns.Count)];
                    break;
                case -90:
                    Debug.Log("Selecting left turn road.");
                    prefab = _leftTurns[Random.Range(0, _leftTurns.Count)];
                    break;
                default:
                    Debug.Log("Error selecting road prefab.");
                    break;
            }

            return prefab;
        }

        private Road InstantiateRoad(Road prefab, Vector3 position)
        {
            Road road = Instantiate(prefab, transform);
            road.transform.SetPositionAndRotation(position, Quaternion.Euler(0, _rotation, 0));

            return road;
        }

        private void GeneratePickups()
        {
            List<Road> roadsWithPickupSpawnpoints = _spawnedRoads.Where(x => x.Pickups.Count > 0).ToList();
            bool spawnCoin = true;
            int count = 0;
            HashSet<int> processed = new HashSet<int>();

            while (count < Mathf.Min(_coinAmount + _itemboxAmount, roadsWithPickupSpawnpoints.Count))
            {
                int rIndex = Random.Range(0, roadsWithPickupSpawnpoints.Count);
                if (processed.Contains(rIndex))
                    continue;

                processed.Add(rIndex);

                count++;
                BasePickup pickupPrefab = spawnCoin ? _pointPrefab : _itemBoxPrefab;
                spawnCoin = !spawnCoin;

                Road road = roadsWithPickupSpawnpoints[rIndex];

                for (int i = 0; i < road.Pickups.Count; i++)
                {
                    Vector3 pointPosition = road.Pickups[i].position;
                    pointPosition.y = 1.0f;

                    GameObject point = _container.InstantiatePrefab(pickupPrefab);
                    point.transform.position = pointPosition;
                    point.transform.parent = road.transform;
                }
            }


        }

        private void CombineColliders()
        {
            MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>().Where(x => x.CompareTag(ROAD_TAG)).ToArray();
            CombineInstance[] combine = new CombineInstance[meshColliders.Length];

            for (int i = 0; i < meshColliders.Length; i++)
            {
                meshColliders[i].enabled = false;
                combine[i].mesh = meshColliders[i].sharedMesh;
                combine[i].transform = meshColliders[i].transform.localToWorldMatrix;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine);
            _meshCollider.sharedMesh = mesh;
            _meshCollider.enabled = true;
        }
    }
}
