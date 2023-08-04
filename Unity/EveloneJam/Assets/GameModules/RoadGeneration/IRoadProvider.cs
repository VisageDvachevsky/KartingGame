using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.RoadGeneration
{
    public interface IRoadProvider
    {
        event Action OnRoadCreated;
        IReadOnlyList<Road> SpawnedRoads { get; }
        IReadOnlyList<RoadCheckpoint> Checkpoints { get; }
        IReadOnlyList<Transform> KartSpawnpoints { get; }
    }
}