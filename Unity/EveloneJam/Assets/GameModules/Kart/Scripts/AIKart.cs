using Project.RoadGeneration;
using UnityEngine;
using Zenject;

namespace Project.Kart
{
    [RequireComponent(typeof(KartController))]
    public class AIKart : MonoBehaviour, IKartInput
    {
        [SerializeField] private float _driftStartAngle = 40f;
        [SerializeField] private float _driftStopAngle = 10f;
        [SerializeField] private float _boostMaxAngle = 5f;
        [SerializeField] private float _maxSteering = 1.5f;
        [SerializeField] private float _steerScaling = 0.2f;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private float _avoidActionCooldown = 2f;

        [SerializeField] private RaycastData[] _raycasts = new RaycastData[4];

        private CheckpointCounter _checkpointCounter;
        private KartController _kart;

        private float _currentHorizontal;
        private float _targetHorizontal;
        private float _angleToCheckpoint;
        private float _driftEnterAngle;
        private IRoadProvider _roadProvider;
        private IInputLock _inputLock;
        private float _nextAvoidActionTime = 0f;

        public bool IsPlayer => false;

        [Inject]
        private void Construct(IRoadProvider roadProvider, IInputLock inputLock)
        {
            _roadProvider = roadProvider;
            _inputLock = inputLock;
        }

        private void Start()
        {
            _kart = GetComponent<KartController>();
            _checkpointCounter = _kart.CheckpointCounter;

            FindAndSetupRaycastOrigins();
        }

        private void Update()
        {
            _currentHorizontal = Mathf.Lerp(_currentHorizontal, _targetHorizontal, Time.deltaTime * 10f);
        }

        private void FindAndSetupRaycastOrigins()
        {
            for (int i = 0; i < _raycasts.Length; i++)
            {
                Transform raycastOrigin = FindChildRecursive(transform, $"{i + 1}");
                if (raycastOrigin != null)
                {
                    _raycasts[i].origin = raycastOrigin;
                }
                else
                {
                    Debug.LogError($"Raycast origin {i} not found in prefab {gameObject.name}");
                }
            }
        }

        private Transform FindChildRecursive(Transform parent, string name)
        {
            Transform result = null;
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    result = child;
                    break;
                }
                else
                {
                    result = FindChildRecursive(child, name);
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        private bool ShouldAvoidCollision()
        {
            for (int i = 0; i < _raycasts.Length; i++)
            {
                RaycastData raycastData = _raycasts[i];
                if (Physics.Raycast(raycastData.origin.position, raycastData.origin.forward, out RaycastHit hit, raycastData.distance, _obstacleLayerMask))
                {
                    return true;
                }
            }

            return false;
        }

        private enum AvoidAction
        {
            Accelerate,
            Decelerate,
            Sidestep
        }

        private AvoidAction ChooseRandomAvoidAction()
        {
            int randomIndex = Random.Range(0, 3);
            return (AvoidAction)randomIndex;
        }

        private void PerformAvoidAction(AvoidAction action)
        {
            switch (action)
            {
                case AvoidAction.Accelerate:
                    // Реализовать ускорение машины
                    Debug.Log("Accelerate");
                    break;
                case AvoidAction.Decelerate:
                    // Реализовать сбавление скорости машины
                    Debug.Log("Decelerate");
                    break;
                case AvoidAction.Sidestep:
                    // Реализовать объезд препятствия (изменение направления движения)
                    Debug.Log("SideStep");
                    break;
                default:
                    break;
            }
        }

        public bool GetBoostButtonPressed()
        {
            if (_inputLock.Locked) return false;

            return Mathf.Abs(_angleToCheckpoint) <= _boostMaxAngle;
        }

        public float GetHorizontal()
        {
            if (_inputLock.Locked) return 0f;

            Vector3 targetPosition = _roadProvider.Checkpoints[(_checkpointCounter.CurrentCheckpoint + 1) % _roadProvider.Checkpoints.Count].WorldPosition;
            targetPosition.y = transform.position.y;
            Vector3 targetDirection = (targetPosition - transform.position).normalized;

            _angleToCheckpoint = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

            if (ShouldAvoidCollision() && Time.time >= _nextAvoidActionTime)
            {
                AvoidAction randomAction = ChooseRandomAvoidAction();
                PerformAvoidAction(randomAction);

                _nextAvoidActionTime = Time.time + _avoidActionCooldown;
            }
            else
            {
                _targetHorizontal = Mathf.Clamp(_angleToCheckpoint * _steerScaling, -_maxSteering, _maxSteering);
            }

            return _currentHorizontal;
        }

        public bool GetJumpButtonDown()
        {
            if (_inputLock.Locked) return false;

            if (!_kart.InDrift && Mathf.Abs(_angleToCheckpoint) > _driftStartAngle)
            {
                _driftEnterAngle = _angleToCheckpoint;
                return true;
            }

            return false;
        }

        public bool GetJumpButtonUp()
        {
            if (_inputLock.Locked) return false;

            if (_kart.InDrift &&
                (Mathf.Sign(_driftEnterAngle) != Mathf.Sign(_angleToCheckpoint)
                || Mathf.Sign(_kart.DriftDirection) != Mathf.Sign(_angleToCheckpoint)
                || Mathf.Abs(_angleToCheckpoint) <= _driftStopAngle))
            {
                return true;
            }

            return false;
        }

        public float GetVertical()
        {
            if (_inputLock.Locked) return 0f;

            return 1f;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);

            Gizmos.color = Color.green;
            if (_roadProvider != null && _checkpointCounter != null)
            {
                Vector3 targetPosition = _roadProvider.Checkpoints[(_checkpointCounter.CurrentCheckpoint + 1) % _roadProvider.Checkpoints.Count].WorldPosition;
                targetPosition.y = transform.position.y;
                Gizmos.DrawLine(transform.position, targetPosition);
            }

            DebugDrawRays();
        }

        private void DebugDrawRays()
        {
            for (int i = 0; i < _raycasts.Length; i++)
            {
                RaycastData raycastData = _raycasts[i];
                if (raycastData == null) continue;

                Transform raycastOrigin = raycastData.origin;
                Vector3 raycastDirection = raycastOrigin.forward;
                Ray ray = new Ray(raycastOrigin.position, raycastDirection);

                Debug.DrawRay(ray.origin, ray.direction * raycastData.distance, Color.red);
            }
        }

        public bool GetItemButtomDown()
        {
            return true;
        }
    }

    [System.Serializable]
    public class RaycastData
    {
        public Transform origin;
        public float distance = 5f;
    }
}
