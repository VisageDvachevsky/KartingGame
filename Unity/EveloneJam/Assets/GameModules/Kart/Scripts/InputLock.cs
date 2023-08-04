using UnityEngine;

namespace Project.Kart
{
    public class InputLock : MonoBehaviour, IInputLock
    {
        public bool Locked { get; private set; }

        public void Lock()
        {
            Locked = true;
        }

        public void Unlock()
        {
            Locked = false;
        }
    }
}