using UnityEngine;

namespace Project.Kart
{
    public abstract class KartEffect : ScriptableObject
    {
        [field: SerializeField] public float Duration { get; private set; } = 3f;
        
        public virtual void Activate(KartController kartController) { }

        public virtual void Deactivate(KartController kartController) { }

        public virtual void Execute(KartController kartController) { }
    }
}