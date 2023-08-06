using DG.Tweening;
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
            if (_checkpointCounter == null)
                return;

            UpdateLapsAmount();
        }

        private void SyncKart(KartController kart)
        {
            _checkpointCounter = kart.GetComponent<CheckpointCounter>();
        }

        private void UpdateLapsAmount()
        {
            string newValue = $"{_checkpointCounter.LapsFinished}/{_checkpointCounter.LapCount}";

            if (newValue == _text.text)
                return;

            _text.transform.DOComplete();
            _text.transform.localScale = Vector3.one;
            _text.transform.DOPunchScale(Vector3.one * 1.2f, .2f, 5, 1);
            _text.text = newValue;
        }
    }
}