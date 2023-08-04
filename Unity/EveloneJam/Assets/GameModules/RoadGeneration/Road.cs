using System.Collections.Generic;
using UnityEngine;

namespace Project.RoadGeneration
{
    [System.Serializable]
    public struct CheckpointData
    {
        public RoadCheckpoint[] Checkpoints;
    }

    [System.Serializable]
    public struct KartSpawnpointData
    {
        public Transform[] Spawnpoints;
    }

    [System.Serializable]
    public struct PickupSpawnpointData
    {
        public Transform[] Spawnpoints;
    }

    public class Road : MonoBehaviour
    {
        public enum RoadType
        {
            Straight,
            LeftTurn,
            RightTurn,
            StraightStart,
        }

        [SerializeField] private RoadType _type;
        [SerializeField] private CheckpointData _checkpointData;
        [SerializeField] private KartSpawnpointData _kartSpawnpoints;
        [SerializeField] private PickupSpawnpointData _pickups;

        public RoadType Type => _type;
        public IReadOnlyList<RoadCheckpoint> Checkpoints => _checkpointData.Checkpoints;
        public IReadOnlyList<Transform> KartSpawnpoints => _kartSpawnpoints.Spawnpoints;
        public IReadOnlyList<Transform> Pickups => _pickups.Spawnpoints;
    }
}
