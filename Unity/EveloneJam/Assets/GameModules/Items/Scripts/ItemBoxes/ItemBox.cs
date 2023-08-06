using Project.Kart;

namespace Project.Interaction
{
    public class ItemBox : BasePickup
    {

        public override void Pickup(KartController sender)
        {
            if (sender.IsPlayer)
                base.Pickup(sender);

            sender.ItemBoxSystem.SelectNewItem();
        }
    }
}
