using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Kart
{
    public class BoostTimeDisplay : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        private PlayerKartProvider _kartProvider;

        [Inject]
        private void Construct(PlayerKartProvider playerKartProvider)
        {
            _kartProvider = playerKartProvider;
        }

        private void Update()
        {
            KartController kart = _kartProvider.Kart;
            if (kart == null)
                return;

            _fill.fillAmount = kart.RemainingBoostTime / kart.MaxBoostTime;
        }
    }
}