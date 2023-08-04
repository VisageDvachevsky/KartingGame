using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Coins
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
            _text.text = amount.ToString();
        }
    }
}