using UnityEngine;

namespace Project.Kart
{
    [CreateAssetMenu]
    public class SpinningEffect : KartEffect
    {
        public override void Activate(KartController kartController)
        {
            base.Activate(kartController);

            kartController.DisableInput = true;
            kartController.IsSpinning = true;
            kartController.HitStop();
        }

        public override void Deactivate(KartController kartController)
        {
            base.Deactivate(kartController);

            kartController.DisableInput = false;
            kartController.IsSpinning = false;
        }
    }
}