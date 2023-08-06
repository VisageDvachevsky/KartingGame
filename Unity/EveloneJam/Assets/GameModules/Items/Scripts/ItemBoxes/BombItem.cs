using Project.Camera;
using Project.Kart;
using Project.RoadGeneration;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class BombItem : Item
    {
        [SerializeField] private SpinningEffect _spinningEffect;
        [SerializeField] private Transform _model;
        [SerializeField] private float _modelRotationSpeed = 270f;
        [SerializeField] private float _kartAimStrength = 0.5f;
        [SerializeField] private float _kartMinAimDistance = 5f;
        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private float _speed = 30f;
        [SerializeField] private float _aliveTime = 20f;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _explosionSound;
        [SerializeField] private ParticleSystem _explosionParticles;

        private Rigidbody _rigidbody;
        private Collider _collider;
        private int _currentCheckpoint;
        private int _checkpointsAmount;
        private float _startTime;
        private bool _exploded;
        private IRoadProvider _roadProvider;

        [Inject]
        private void Construct(IRoadProvider roadProvider)
        {
            _roadProvider = roadProvider;
        }

        public override bool TryActivate(KartController owner)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            _currentCheckpoint = owner.CheckpointCounter.NextCheckpoint.Index;
            _checkpointsAmount = _roadProvider.Checkpoints.Count;

            _startTime = Time.time;

            transform.SetPositionAndRotation(owner.transform.TransformPoint(_spawnOffset), owner.transform.rotation);

            return true;
        }

        private void Update()
        {
            if (_exploded) return;

            Vector3 velocity = transform.forward * _speed;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
            _model.Rotate(0, _modelRotationSpeed * Time.deltaTime, 0, Space.Self);

            Aim();

            if (Time.time - _startTime >= _aliveTime)
                ExplodeOn(null);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out RoadGuard roadGuard))
            {
                Vector3 checkpoint = _roadProvider.Checkpoints[_currentCheckpoint].WorldPosition;
                Vector3 direction = checkpoint - transform.position;
                direction.y = 0f;
                direction.Normalize();

                _rigidbody.velocity = Vector3.zero;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _rigidbody.rotation = targetRotation;
            }

            if (collision.transform.TryGetComponent(out KartTrigger kartTrigger))
            {
                ExplodeOn(kartTrigger.Kart);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out RoadCheckpoint checkpoint))
            {
                _currentCheckpoint = (checkpoint.Index + 1) % _checkpointsAmount;
            }
        }

        private void Aim()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _kartMinAimDistance);
            KartTrigger nearest = null;
            float minSqrDistance = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                KartTrigger kartTrigger = colliders[i].GetComponent<KartTrigger>();
                if (kartTrigger == null)
                    return;

                float sqrDistance = (transform.position - colliders[i].transform.position).sqrMagnitude;
                
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    nearest = kartTrigger;
                }
            }

            if (nearest != null)
            {
                Vector3 direction = (nearest.transform.position - transform.position);
                direction.y = 0;
                direction.Normalize();

                Quaternion rotation = Quaternion.LookRotation(direction);

                _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, rotation, Time.deltaTime * _kartAimStrength);
            }
        }

        private void ExplodeOn(KartController kart)
        {
            _exploded = true;

            if (kart != null)
            {
                kart.EffectHandler.AddEffect(_spinningEffect);
            }


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
            Gizmos.DrawWireSphere(transform.position, _kartMinAimDistance);
        }
    }
}
