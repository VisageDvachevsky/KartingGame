using Project.Kart;

namespace Project.Interaction
{
    public interface IPickup
    {
        bool PlayerOnly { get; }
        void Pickup(KartController sender);
    }
}