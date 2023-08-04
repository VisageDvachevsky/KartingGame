using Project.Kart;
using Project.RoadGeneration;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Laps
{
    public class LapCountDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;


        private PlayerKartProvider _kartProvider;
        private CheckpointCounter _checkpointCounter;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider)
        {
            _kartProvider = kartProvider;
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
                UpdateLapsAmount();
            }
        }

        private void SyncKart(KartController kart)
        {
            _checkpointCounter = kart.GetComponent<CheckpointCounter>();
        }

        private void UpdateLapsAmount()
        {
            _text.text = $"{_checkpointCounter.LapsFinished}/{_checkpointCounter.LapCount}";
        }
    }
}