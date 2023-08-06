using Project.Camera;
using Project.Kart;
using Project.Laps;
using Project.RoadGeneration;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RocketItem : Item
    {
        [SerializeField] private SpinningEffect _spinningEffect;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private float _targetDistance = 4f;
        [SerializeField] private Vector3 _checkpointOffset = new Vector3(0f, 1.5f, 0f);
        [SerializeField] private float _checkpointCatchDistance = 4f;
        [SerializeField] private float _aimingStrength = 10f;
        [SerializeField] private float _movementSpeed = 10f;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private ScoreSystem _scoreSystem;
        private IRoadProvider _roadProvider;
        private KartController _target;
        private bool _exploded = false;
        private int _currentCheckpoint;
        private int _checkpointsAmount;

        [Inject]
        private void Construct(ScoreSystem scoreSystem, IRoadProvider roadProvider)
        {
            _scoreSystem = scoreSystem;
            _roadProvider = roadProvider;
        }

        public override bool TryActivate(KartController owner)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _currentCheckpoint = owner.CheckpointCounter.NextCheckpoint.Index;
            _checkpointsAmount = _roadProvider.Checkpoints.Count;

            UpdateTarget();
            return true;
        }

        private void Update()
        {
            if (_exploded)
                return;

            _model.Rotate(0, 0, _rotationSpeed * Time.deltaTime, Space.Self);

            UpdateTarget();

            float sqrDistanceToTarget = (transform.position - _target.transform.position).sqrMagnitude;
            float sqrAimDistance = _targetDistance * _targetDistance;

            Vector3 targetPosition;
            if (sqrDistanceToTarget <= sqrAimDistance) {
                targetPosition = _target.transform.position;
            }
            else
            {
                targetPosition = _roadProvider.Checkpoints[_currentCheckpoint].WorldPosition
                    + _checkpointOffset;

                float sqrDistanceToCheckpoint = (transform.position - targetPosition).sqrMagnitude;
                float checkpointCatchSqrDistance = _checkpointCatchDistance * _checkpointCatchDistance;

                if (sqrDistanceToCheckpoint <= checkpointCatchSqrDistance)
                    _currentCheckpoint = (_currentCheckpoint + 1) % _checkpointsAmount;
            }

            MoveToTargetPosition(targetPosition);
        }

        private void MoveToTargetPosition(Vector3 targetPosition)
        {
            Vector3 normalizedDirection = (targetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(normalizedDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _aimingStrength);

            transform.position += transform.forward * Time.deltaTime * _movementSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out KartTrigger kartTrigger))
            {
                KartController kart = kartTrigger.Kart;

                if (kart == _target)
                {
                    ExplodeOn(_target);
                }
            }
        }

        private void UpdateTarget()
        {
            _target = _scoreSystem.ScoreDatas[0].CheckpointCounter.Kart;
        }

        private void ExplodeOn(KartController kart)
        {
            _exploded = true;

            kart.EffectHandler.AddEffect(_spinningEffect);

            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            _explosionParticles.Play();

            _audio.Stop();
            _audio.PlayOneShot(_explosionSound);

            Destroy(gameObject, _explosionParticles.main.duration);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _targetDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _checkpointCatchDistance);
        }
    }
}
