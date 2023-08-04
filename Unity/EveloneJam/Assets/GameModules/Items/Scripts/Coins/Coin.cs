using Project.Interaction;
using Project.Kart;
using Zenject;

namespace Project.Coins
{
    public class Coin : BasePickup
    {
        private CoinSystem _coinSystem;

        [Inject]
        private void Construct(CoinSystem coinSystem)
        {
            _coinSystem = coinSystem;
        }

        public override void Pickup(KartController sender)
        {
            base.Pickup(sender);
            _coinSystem.AddCoin();
        }
    }
}
