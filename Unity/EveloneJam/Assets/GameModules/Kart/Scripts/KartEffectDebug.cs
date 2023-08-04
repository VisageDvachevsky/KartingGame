using UnityEngine;

namespace Project.Kart
{
    [RequireComponent(typeof(KartController))]
    public class KartEffectDebug : MonoBehaviour
    {
        [SerializeField] private KartEffect _effect;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GetComponent<KartController>().EffectHandler.AddEffect(_effect);
            }
        }
    }
}