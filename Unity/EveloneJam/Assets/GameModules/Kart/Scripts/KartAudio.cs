using UnityEngine;

namespace Project.Kart
{
    public class KartAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _engineAudioSource;
        [SerializeField] private AudioSource _hitAudioSource;
        [SerializeField] private float _pitchMultiplier = 0.035f;

        private KartController _kart;
        private float _basePitch;

        private void Awake()
        {
            _kart = GetComponentInParent<KartController>();
            _basePitch = _engineAudioSource.pitch;
        }

        private void OnEnable()
        {
            _kart.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            _kart.OnHit -= HandleHit;
        }

        private void HandleHit()
        {
            _hitAudioSource.Play();
        }

        private void Update()
        {
            Vector3 velocity = _kart.CurrentVelocity;
            float speed = velocity.z;
            
            _engineAudioSource.pitch = _basePitch + speed * _pitchMultiplier;
        }
    }
}