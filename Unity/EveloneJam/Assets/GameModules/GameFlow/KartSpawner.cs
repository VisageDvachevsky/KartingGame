using Project.Kart;
using Project.RoadGeneration;
using UnityEngine;
using Zenject;
using Project.Camera;
using System.Collections.Generic;
using Project.Laps;

public class KartSpawner : MonoBehaviour
{
    [SerializeField] private KartController _playerKartPrefab;
    [SerializeField] private KartController[] _aiKartPrefabs;

    private PlayerKartProvider _kartProvider;
    private IRoadProvider _roadProvider;
    private MapSettings _mapSettings;
    private DiContainer _container;

    [Inject]
    private void Construct(DiContainer container,
        PlayerKartProvider kartProvider,
        IRoadProvider roadProvider,
        MapSettings mapSettings)
    {
        _container = container;
        _kartProvider = kartProvider;
        _roadProvider = roadProvider;
        _mapSettings = mapSettings;
    }

    public void RespawnKart()
    {
        if (_kartProvider.Kart)
        {
            Destroy(_kartProvider.Kart.gameObject);
        }

        IReadOnlyList<Transform> spawnpoints = _roadProvider.KartSpawnpoints;
        int playerSpawnpointIndex = Random.Range(0, spawnpoints.Count);
        KartController kart = SpawnKartAt(spawnpoints[playerSpawnpointIndex], _playerKartPrefab);
        _kartProvider.SetKart(kart);

        int spawned = 0;
        for (int i = 0; i < spawnpoints.Count; i++)
        {
            if (spawned >= _mapSettings.BotCount) break;
            if (i == playerSpawnpointIndex) continue;

            SpawnKartAt(spawnpoints[i], _aiKartPrefabs[Random.Range(0, _aiKartPrefabs.Length)]);
            spawned++;
        }
    }

    private KartController SpawnKartAt(Transform spawnpoint, KartController prefab)
    {
        KartController kart = _container.InstantiatePrefabForComponent<KartController>(prefab);
        kart.transform.position = spawnpoint.position;
        kart.transform.rotation = spawnpoint.rotation;

        return kart;
    }
}
