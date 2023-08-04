using DG.Tweening;
using Project.Interaction;
using Project.RoadGeneration;
using Project.Utils;
using System;
using UnityEngine;

namespace Project.Kart
{
    [RequireComponent(typeof(CheckpointCounter), typeof(KartEffectHandler), typeof(ItemBoxSystem))]
    public class KartController : MonoBehaviour
    {
        private const float ACCELERATION_MULTIPLIER = 1000f;

        [SerializeField] private KartTrigger _kartTrigger;
        [SerializeField] private Transform _model;
        [SerializeField] private Transform _groundRayPoint;
        [SerializeField] private float _groundRayLength = 0.1f;
        [SerializeField] private LayerMask _groundMask = Physics.DefaultRaycastLayers;
        [Header("Boost")]
        [SerializeField] private float _maxBoostTime = 8f;
        [SerializeField] private float _boostAccumulationSpeed = 2f;
        [Header("Settings")]
        [SerializeField] public float ForwardAccel = 8f;
        [SerializeField] public float ReverseAccel = 4f;
        [SerializeField] public float DriftSteering = 3f;
        [SerializeField] public float DriftAccel = 10f;
        [SerializeField] public float BoostAccelMultiplier = 2f;
        [SerializeField] public float AutoStopping = 5f;
        [SerializeField] public float GravityForce = 10f;
        [SerializeField] public float TurningStrength = 180f;
        [SerializeField] public float TurningDebaf = 0.01f;
        [SerializeField] public float JumpHeight = 0.2f;
        [SerializeField] public float DriftAngle = 20f;
        [SerializeField] public float DragOnGround = 3f;
        [SerializeField] public float MinHitSpeed = 3f;
        [SerializeField] public float HitKnockbackStrength = 3f;
        [SerializeField] public float SpinningAngularSpeed = 360f;

        public event Action OnHit;

        private CheckpointCounter _checkpointCounter;
        private KartEffectHandler _effectHandler;
        private ItemBoxSystem _itemBoxSystem;
        private IKartInput _kartInput;
        private Rigidbody _sphereRb;
        private float _speedInput;
        private float _currentSpeed;
        private float _remainingBoostTime;

        public CheckpointCounter CheckpointCounter
        {
            get {
                if (!_checkpointCounter) _checkpointCounter = GetComponent<CheckpointCounter>();
                return _checkpointCounter;
            }
        }

        public KartEffectHandler EffectHandler
        {
            get
            {
                if (!_effectHandler) _effectHandler = GetComponent<KartEffectHandler>();
                return _effectHandler;
            }
        }

        public ItemBoxSystem ItemBoxSystem
        {
            get
            {
                if (!_itemBoxSystem) _itemBoxSystem = GetComponent<ItemBoxSystem>();
                return _itemBoxSystem;
            }
        }

        public Vector3 CurrentVelocity => transform.InverseTransformDirection(_sphereRb.velocity);
        public float CurrentSteering { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool InBoost { get; private set; }
        public float MaxBoostTime => _maxBoostTime;
        public int DriftDirection { get; private set; }
        public bool InDrift => DriftDirection != 0;
        public float RemainingBoostTime {
            get => _remainingBoostTime;
            set => _remainingBoostTime = Mathf.Clamp(value, 0, _maxBoostTime);
        }
        public bool DisableInput { get; set; } = false;
        public bool IsPlayer => _kartInput.IsPlayer;
        public bool IsSpinning { get; set; } = false;
        public Transform Model => _model;

        private float Horizontal => DisableInput ? 0f : _kartInput.GetHorizontal();
        private float Vertical => DisableInput ? 0f : _kartInput.GetVertical();


        private void Start()
        {
            Physics.defaultContactOffset = 1e-4f;

            EffectHandler.Kart = this;
            CheckpointCounter.Kart = this;
            ItemBoxSystem.Kart = this;
            _kartTrigger.Kart = this;


            RemainingBoostTime = _maxBoostTime;

            _sphereRb = _kartTrigger.Rigidbody;
            _sphereRb.transform.SetParent(null);
        }

        private void Update()
        {
            transform.position = _sphereRb.transform.position;
            if (IsSpinning)
            {
                _model.localRotation = Quaternion.Euler(0, _model.localEulerAngles.y + SpinningAngularSpeed * Time.deltaTime, 0);
            }
            else
            {
                _model.localRotation = Quaternion.Slerp(_model.localRotation, Quaternion.Euler(0, DriftDirection * DriftAngle, 0), Time.deltaTime * 4f);
            }

            float verticalInput = Vertical;
            float horizontalInput = Horizontal;
            _speedInput = CalculateTargetSpeed(verticalInput);
            _currentSpeed = _speedInput;

            if (RemainingBoostTime > 0f && verticalInput > 0f && DriftDirection == 0 && _kartInput.GetBoostButtonPressed())
            {
                InBoost = true;
                RemainingBoostTime -= Time.deltaTime;
                if (RemainingBoostTime < 0f) RemainingBoostTime = 0f;
            }
            else
            {
                InBoost = false;
            }

            if (InDrift)
            {
                RemainingBoostTime += _boostAccumulationSpeed * Time.deltaTime;
                if (RemainingBoostTime > _maxBoostTime) RemainingBoostTime = _maxBoostTime;
            }


            if (DriftDirection == 0 && _kartInput.GetJumpButtonDown() && CurrentVelocity.z > 10f)
            {
                if (horizontalInput < 0f) DriftDirection = -1;
                else if (horizontalInput > 0f) DriftDirection = 1;

                if (DriftDirection != 0)
                {
                    _model.DOComplete();
                    _model.DOPunchPosition(transform.up * JumpHeight, .3f, 5, 1);
                }
            }
            if (DriftDirection != 0 && (_kartInput.GetJumpButtonUp() || CurrentVelocity.z < 10f))
            {
                DriftDirection = 0;
            }

            Turn(verticalInput, horizontalInput);

            if (!DisableInput && _kartInput.GetItemButtomDown())
            {
                _itemBoxSystem.ActivateCurrentItem();
            }
        }

        private void FixedUpdate()
        {
            IsGrounded = RaycastGround(out RaycastHit hit);

            if (IsGrounded)
            {
                _model.rotation = Quaternion.FromToRotation(_model.up, hit.normal) * _model.rotation;
                _sphereRb.drag = DragOnGround;

                if (Mathf.Abs(_speedInput) > 0)
                {
                    _sphereRb.AddForce(transform.forward * _currentSpeed);
                }
            }
            else
            {
                _sphereRb.drag = 0.1f;
                _sphereRb.AddForce(Vector3.up * -GravityForce * ACCELERATION_MULTIPLIER);
            }
        }

        public void SetInput(IKartInput kartInput)
        {
            _kartInput = kartInput;
        }

        public void CollideWithOtherKart(Vector3 normal, Vector3 relativeVelocity)
        {
            if (relativeVelocity.magnitude < MinHitSpeed) return;

            normal.y = 0f;
            normal.Normalize();

            _sphereRb.velocity = new Vector3(normal.x * HitKnockbackStrength, 0f, normal.z * HitKnockbackStrength);

            OnHit?.Invoke();
        }

        public void HitStop()
        {
            Vector3 normalizedVelocity = _sphereRb.velocity.normalized * HitKnockbackStrength;
            _sphereRb.velocity = new Vector3(normalizedVelocity.x, 0f, normalizedVelocity.z);

            OnHit?.Invoke();
        }

        public void CollideWithRoadGuard(Vector3 normal, Vector3 relativeVelocity)
        {

            RoadCheckpoint nextCheckpoint = CheckpointCounter.NextCheckpoint;
            Vector3 closestPoint = nextCheckpoint.WorldPosition;
            Vector3 targetDirection = closestPoint - transform.position;

            transform.rotation =  Quaternion.LookRotation(targetDirection);

            if (relativeVelocity.magnitude < MinHitSpeed) return;

            normal.y = 0f;
            normal.Normalize();
            
            _sphereRb.velocity = new Vector3(normal.x * HitKnockbackStrength, _sphereRb.velocity.y, normal.z * HitKnockbackStrength);


            OnHit?.Invoke();
        }

        private bool RaycastGround(out RaycastHit hit)
        {
            return Physics.Raycast(_groundRayPoint.position, -transform.up, out hit, _groundRayLength, _groundMask);
        }

        private void Turn(float verticalInput, float horizontalInput)
        {
            if (DriftDirection == -1)
            {
                horizontalInput = NumericUtils.Remap(horizontalInput, -1, 1, -DriftSteering, 0);
            }
            else if (DriftDirection == 1)
            {
                horizontalInput = NumericUtils.Remap(horizontalInput, -1, 1, 0, DriftSteering);
            }

            if (IsGrounded)
            {
                float turningAmount = horizontalInput * verticalInput;
                if (DriftDirection == 0 && CurrentVelocity.z > 20f)
                {
                    turningAmount *= TurningDebaf;
                }

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                    + new Vector3(0f, TurningStrength * turningAmount * Time.deltaTime, 0f));
            }

            CurrentSteering = horizontalInput;
        }

        private float CalculateTargetSpeed(float verticalInput)
        {
            float speedInput = 0;
            if (verticalInput > 0)
            {
                speedInput = verticalInput * (InDrift ? DriftAccel : ForwardAccel) * ACCELERATION_MULTIPLIER;
            }
            else if (verticalInput < 0)
            {
                speedInput = verticalInput * ReverseAccel * ACCELERATION_MULTIPLIER;
            }

            if (InBoost)
            {
                speedInput *= BoostAccelMultiplier;
            }

            return speedInput;
        }
    }
}