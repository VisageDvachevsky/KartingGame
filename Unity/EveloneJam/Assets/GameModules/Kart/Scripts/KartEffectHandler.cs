using UnityEngine;

namespace Project.Kart
{
    public class KartEffectHandler : MonoBehaviour
    {
        public KartController Kart { get; set; }
        public KartEffect CurrentEffect { get; private set; }
        public float EffectStartTime { get; private set; }

        private void Update()
        {
            HandleEffect();
        }

        public void AddEffect(KartEffect kartEffect)
        {
            if (CurrentEffect != null)
                return;

            CurrentEffect = kartEffect;
            EffectStartTime = Time.time;
            CurrentEffect.Activate(Kart);
        }

        private void HandleEffect()
        {
            if (CurrentEffect == null)
                return;

            CurrentEffect.Execute(Kart);

            if (Time.time - EffectStartTime >= CurrentEffect.Duration)
            {
                CurrentEffect.Deactivate(Kart);
                CurrentEffect = null;
            }
        }
    }
}
