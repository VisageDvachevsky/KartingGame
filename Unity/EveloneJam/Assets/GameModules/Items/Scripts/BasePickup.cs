using DG.Tweening;
using Project.Kart;
using UnityEngine;

namespace Project.Interaction
{
    public class BasePickup : MonoBehaviour, IPickup
    {
        [SerializeField] private float _respawnTime = 5.0f;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private ParticleSystem _pickupParticles;
        [SerializeField] private AudioSource _audio;

        public virtual bool PlayerOnly => true;

        private void Update()
        {
            _model.Rotate(0, _rotationSpeed * Time.deltaTime, 0, Space.Self);
        }

        public virtual void Pickup(KartController sender)
        {
            HideAndRespawn();
            _audio.Play();
            _pickupParticles.Play();
        }

        private void HideAndRespawn()
        {
            _collider.enabled = false;
            _model.DOMoveY(_model.position.y + 1f, .5f);
            _model.DOScale(Vector3.zero, .5f);

            Invoke(nameof(Respawn), _respawnTime);
        }

        private void Respawn()
        {
            _collider.enabled = true;
            _model.DOMoveY(_model.position.y - 1f, .5f);
            _model.DOScale(Vector3.one, .5f);
        }
    }
}