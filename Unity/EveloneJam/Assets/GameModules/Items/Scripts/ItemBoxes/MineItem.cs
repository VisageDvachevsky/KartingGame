using Project.Camera;
using Project.Kart;
using UnityEngine;
using Zenject;

namespace Project.Interaction
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class MineItem : Item
    {
        [SerializeField] private SpinningEffect _spinningEffect;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private Vector3 _startOffset = new Vector3(0, 3f, 0);
        [SerializeField] private Vector3 _startVelocity = new Vector3(0, 2f, 0);

        private Rigidbody _rigidbody;
        private Collider _collider;

        public override bool TryActivate(KartController owner)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();

            transform.Translate(_startOffset);
            _rigidbody.velocity = _startVelocity;

            return true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out KartTrigger kartTrigger))
            {
                kartTrigger.Kart.EffectHandler.AddEffect(_spinningEffect);

                _rigidbody.isKinematic = true;
                _collider.enabled = false;
                _particles.Play();
                _audio.Play();

                Destroy(gameObject, _particles.main.duration);
            }
        }
    }
}