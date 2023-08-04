using Project.RoadGeneration;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Laps
{
    public class ScoreSystem : MonoBehaviour
    {
        public class ScoreData
        {
            public CheckpointCounter CheckpointCounter;
            public int AbsoluteCheckpoint;
            public float DistanceToNextCheckpoint;
        }

        private List<ScoreData> _scoreDatas = new List<ScoreData>();
        private IRoadProvider _roadProvider;

        public IReadOnlyList<ScoreData> ScoreDatas => _scoreDatas;

        [Inject]
        private void Construct(IRoadProvider roadProvider)
        {
            _roadProvider = roadProvider;
        }

        public void Register(CheckpointCounter checkpointCounter)
        {
            _scoreDatas.Add(new ScoreData
            {
                CheckpointCounter = checkpointCounter,
                AbsoluteCheckpoint = 0,
                DistanceToNextCheckpoint = 0
            });
        }

        public void Unregister(CheckpointCounter checkpointCounter)
        {
            _scoreDatas.RemoveAll(x => x.CheckpointCounter == checkpointCounter);
        }

        public int GetPlace(CheckpointCounter checkpointCounter)
        {
            int place = -1;

            for (int i = 0; i < _scoreDatas.Count; i++)
            {
                ScoreData scoreData = _scoreDatas[i];

                if (scoreData.CheckpointCounter == checkpointCounter)
                {
                    place = i + 1;
                    break;
                }
            }

            return place;
        }

        private void Update()
        {
            IReadOnlyList<RoadCheckpoint> checkpoints = _roadProvider.Checkpoints;
            for (int i = 0; i < _scoreDatas.Count; i++)
            {
                CheckpointCounter counter = _scoreDatas[i].CheckpointCounter;
                _scoreDatas[i].AbsoluteCheckpoint = counter.AbsoluteCheckpointIndex;
                float distanceToCheckpoint = Vector3.Distance(
                    counter.transform.position,
                    checkpoints[(counter.CurrentCheckpoint + 1) % checkpoints.Count].WorldPosition);
                _scoreDatas[i].DistanceToNextCheckpoint = distanceToCheckpoint;
            }

            _scoreDatas = _scoreDatas.OrderByDescending(x => x.AbsoluteCheckpoint)
                .ThenBy(x => x.DistanceToNextCheckpoint)
                .ToList();
        }
    }
}
