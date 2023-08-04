using UnityEngine;
using Zenject;

namespace Project.Kart
{
    public class PlayerInput : MonoBehaviour, IKartInput
    {
        private IInputLock _inputLock;

        public bool IsPlayer => true;

        [Inject]
        private void Construct(IInputLock inputLock)
        {
            _inputLock = inputLock;
        } 

        public float GetHorizontal() => _inputLock.Locked ? 0f : Input.GetAxis("Horizontal");
        public float GetVertical() => _inputLock.Locked ? 0f : (Input.GetAxisRaw("Vertical") < 0f ? 0f : 1f);
        public bool GetJumpButtonDown() => _inputLock.Locked ? false : Input.GetButtonDown("Jump");
        public bool GetJumpButtonUp() => _inputLock.Locked ? false : Input.GetButtonUp("Jump");
        public bool GetBoostButtonPressed() => _inputLock.Locked ? false : Input.GetButton("Fire1");

        public bool GetItemButtomDown() => _inputLock.Locked ? false : Input.GetButtonDown("Use Item");
    }
}