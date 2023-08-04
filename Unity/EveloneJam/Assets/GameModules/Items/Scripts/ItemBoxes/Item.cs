using Project.Kart;
using UnityEngine;

namespace Project.Interaction
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void Activate(KartController owner);
    }
}
