using System;
using UnityEngine;
using Zenject;
using Project.Laps;
using Project.Kart;
using Project.GameFlow;

namespace Project.RoadGeneration
{
    public class CheckpointCounter : MonoBehaviour
    {
        private int _checkpointsAmount;

        private IRoadProvider _roadProvider;
        private ScoreSystem _scoreSystem;

        public event Action<int> LapFinished;
        public event Action AllLapsFinished;

        public KartController Kart { get; set; }
        public int LapCount { get; private set; }
        public int LapsFinished { get; private set; }
        public bool Finished => LapsFinished >= LapCount;

        public int CurrentCheckpoint { get; private set; }
        public int AbsoluteCheckpointIndex => LapsFinished * _checkpointsAmount + CurrentCheckpoint;
        public RoadCheckpoint NextCheckpoint => _roadProvider.Checkpoints[(CurrentCheckpoint + 1) % _roadProvider.Checkpoints.Count];

        [Inject]
        private void Construct(MapSettings mapSettings, IRoadProvider roadProvider, ScoreSystem scoreSystem)
        {
            LapCount = mapSettings.LapCount;
            _roadProvider = roadProvider;
            _scoreSystem = scoreSystem;
            SyncRoadData();
        }

        private void Start()
        {
            CurrentCheckpoint = 0;
            LapsFinished = 0;
            _scoreSystem.Register(this);
        }

        private void OnEnable()
        {
            _roadProvider.OnRoadCreated += SyncRoadData;
        }

        private void OnDisable()
        {
            _roadProvider.OnRoadCreated -= SyncRoadData;
        }

        private void OnDestroy()
        {
            _scoreSystem.Unregister(this);
        }

        private void SyncRoadData()
        {
            _checkpointsAmount = _roadProvider.Checkpoints.Count;
        }

        public void ReachCheckpoint(int index)
        {
            if (index == 0 && CurrentCheckpoint == _checkpointsAmount - 1)
                FinishLap();

            if (index == 0 || index > CurrentCheckpoint)
                CurrentCheckpoint = index;
        }   

        private void FinishLap()
        {
            LapsFinished++;

            if (LapsFinished >= LapCount)
            {
                LapsFinished = LapCount;
                AllLapsFinished?.Invoke();
            }

            LapFinished?.Invoke(LapsFinished);
        }
    }
}