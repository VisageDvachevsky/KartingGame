using Project.Kart;
using UnityEngine;
using Zenject;

namespace Project.Coins
{
    public class KartCoinBonus : MonoBehaviour
    {
        [SerializeField] private KartController _kart;

        private CoinSystem _coinSystem;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
            _coinSystem.CoinsAmountChanged += UpdateBonus;
        }

        private void OnDestroy()
        {
            if (_coinSystem != null) _coinSystem.CoinsAmountChanged -= UpdateBonus;
        }

        private void UpdateBonus(int amount)
        {
            if (amount % 5 == 0)
            {
                _kart.RemainingBoostTime += 0.5f;
            }
        }
    }
}
