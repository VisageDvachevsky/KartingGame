using Project.Kart;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    public class KartCoinBonus : MonoBehaviour
    {
        [SerializeField] private KartController _kart;

        private CoinSystem _coinSystem;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
        }

        private void OnEnable()
        {
            _coinSystem.CoinsAmountChanged += UpdateBonus;
        }

        private void OnDisable()
        {
            _coinSystem.CoinsAmountChanged -= UpdateBonus;
        }

        private void UpdateBonus(int amount)
        {
            if (amount % 5 == 0)
            {
                _kart.RemainingBoostTime = _kart.MaxBoostTime;
            }
        }
    }
}
