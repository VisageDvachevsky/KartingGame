using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    public class CoinDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private CoinSystem _coinSystem;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
        }

        private void Start()
        {
            UpdateCoinsAmount(_coinSystem.CoinsAmount);
        }

        private void OnEnable()
        {
            _coinSystem.CoinsAmountChanged += UpdateCoinsAmount;
        }

        private void OnDisable()
        {
            _coinSystem.CoinsAmountChanged -= UpdateCoinsAmount;
        }

        private void UpdateCoinsAmount(int amount)
        {
            string newValue = amount.ToString();

            if (newValue == _text.text)
                return;

            _text.transform.DOComplete();
            _text.transform.localScale = Vector3.one;
            _text.transform.DOPunchScale(Vector3.one * 1.2f, .2f, 5, 1);
            _text.text = newValue;

        }
    }
}