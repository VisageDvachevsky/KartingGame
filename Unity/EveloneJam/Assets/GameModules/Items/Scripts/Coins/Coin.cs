using Project.Kart;
using TMPro;
using Zenject;

namespace Project.Interaction
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
            if (!sender.IsPlayer)
                return;

            base.Pickup(sender);

            _coinSystem.AddCoin();
        }
    }
}
