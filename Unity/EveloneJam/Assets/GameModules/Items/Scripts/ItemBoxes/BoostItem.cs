using Project.Kart;
using UnityEngine;

namespace Project.Interaction
{
    public class BoostItem : Item
    {
        [SerializeField] private ScaleEffect _effect;

        public override bool TryActivate(KartController owner)
        {
            if (owner.EffectHandler.CurrentEffect != null)
                return false;

            owner.EffectHandler.AddEffect(_effect);
            return true;
        }
    }
}
