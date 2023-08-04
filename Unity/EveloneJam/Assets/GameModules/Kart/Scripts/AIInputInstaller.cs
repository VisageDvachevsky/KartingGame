using UnityEngine;

namespace Project.Kart
{
    [RequireComponent(typeof(AIKart))]
    public class AIInputInstaller : MonoBehaviour
    {
        [SerializeField] private AIKart _ai;

        private void Start()
        {
            GetComponent<KartController>().SetInput(_ai);
        }
    }
}
