using System;

namespace Project.Kart
{
    public class PlayerKartProvider
    {
        public event Action<KartController> KartUpdated;
        public KartController Kart { get; private set; }

        public void SetKart(KartController kart)
        {
            Kart = kart;
            KartUpdated?.Invoke(kart);
        }
    }
}