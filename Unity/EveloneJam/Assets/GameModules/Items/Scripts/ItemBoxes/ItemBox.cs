using Project.Kart;

namespace Project.Interaction
{
    public class ItemBox : BasePickup
    {
        //TODO: Возможность использования ботами
        public override bool PlayerOnly => true;

        public override void Pickup(KartController sender)
        {
            base.Pickup(sender);
            sender.ItemBoxSystem.SelectNewItem();
        }
    }
}
