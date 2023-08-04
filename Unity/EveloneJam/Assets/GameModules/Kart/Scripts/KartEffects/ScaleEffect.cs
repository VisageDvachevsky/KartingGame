using DG.Tweening;
using UnityEngine;

namespace Project.Kart
{
    [CreateAssetMenu]
    public class ScaleEffect : KartEffect
    {
        [SerializeField] private float _scale = 0.75f;

        public override void Activate(KartController kartController)
        {
            base.Activate(kartController);

            kartController.Model.DOScale(Vector3.one * _scale, 0.3f).OnComplete(() => kartController.ForwardAccel *= _scale);
        }

        public override void Deactivate(KartController kartController)
        {
            base.Deactivate(kartController);

            kartController.ForwardAccel /= _scale;
            kartController.Model.DOScale(Vector3.one, 0.3f);
        }
    }
}