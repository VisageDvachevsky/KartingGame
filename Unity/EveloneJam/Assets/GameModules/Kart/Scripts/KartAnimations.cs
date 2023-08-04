using UnityEngine;

namespace Project.Kart
{
    public class KartAnimations : MonoBehaviour
    {
        private readonly int AnimatorSpeedParameter = Animator.StringToHash("Speed");

        [SerializeField] private Animator _animator;
        [SerializeField] private float _animatorSpeedMultiplier;
        [SerializeField] private float _wheelSpeedMultiplier;
        [Header("Wheels")]
        [SerializeField] private Transform _leftFrontWheel;
        [SerializeField] private Transform _rightFrontWheel;
        [SerializeField] private float _maxWheelTurn = 30f;
        [SerializeField] private Transform[] _wheels;

        private KartController _kartController;

        private void Start()
        {
            _kartController = GetComponentInParent<KartController>();
        }

        private void Update()
        {
            float speed = _kartController.CurrentVelocity.magnitude;

            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].Rotate(0, 0, speed * _wheelSpeedMultiplier, Space.Self);
            }

            _animator.SetFloat(AnimatorSpeedParameter, speed * _animatorSpeedMultiplier);

            RotateWheels(_kartController.CurrentSteering);
        }

        private void RotateWheels(float horizontalInput)
        {
            _leftFrontWheel.localRotation = Quaternion.Euler(_leftFrontWheel.localEulerAngles.x,
                (horizontalInput * _maxWheelTurn) - 180f, _leftFrontWheel.localEulerAngles.z);
            _rightFrontWheel.localRotation = Quaternion.Euler(_rightFrontWheel.localEulerAngles.x,
                (horizontalInput * _maxWheelTurn), _rightFrontWheel.localEulerAngles.z);
        }
    }
}