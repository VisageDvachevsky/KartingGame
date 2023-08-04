using Project.Kart;
using Project.RoadGeneration;
using Project.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Laps
{
    public class PlaceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private PlayerKartProvider _kartProvider;
        private CheckpointCounter _checkpointCounter;
        private ScoreSystem _scoreSystem;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider, ScoreSystem scoreSystem)
        {
            _kartProvider = kartProvider;
            _scoreSystem = scoreSystem;
        }

        private void OnEnable()
        {
            _kartProvider.KartUpdated += SyncKart;
        }

        private void OnDisable()
        {
            _kartProvider.KartUpdated -= SyncKart;
        }

        private void Update()
        {
            if (_checkpointCounter != null)
            {
                UpdatePlace();
            }
        }

        private void SyncKart(KartController kart)
        {
            _checkpointCounter = kart.CheckpointCounter;
        }

        private void UpdatePlace()
        {
            int place = -1;
            IReadOnlyList<ScoreSystem.ScoreData> scores = _scoreSystem.ScoreDatas;
            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i].CheckpointCounter == _checkpointCounter)
                {
                    place = i + 1;
                    break;
                }
            }

            if (place != -1)
            {
                _text.text = $"{place}{StringNumericUtils.GetOrdinalSuffix(place)}";
            }
        }
    }
}