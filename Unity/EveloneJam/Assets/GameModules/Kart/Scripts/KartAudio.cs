using Project.Interaction;
using UnityEngine;

namespace Project.Kart
{
    public class KartAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _engineAudioSource;
        [SerializeField] private AudioSource _hitAudioSource;
        [SerializeField] private AudioSource _itemAudioSource;
        [SerializeField] private AudioSource _lapFinishAudioSource;
        [SerializeField] private AudioSource _finishAudioSource;
        [SerializeField] private float _pitchMultiplier = 0.035f;

        private KartController _kart;
        private float _basePitch;
        private bool _finished;

        private void Awake()
        {
            _kart = GetComponentInParent<KartController>();
            _basePitch = _engineAudioSource.pitch;
        }

        private void OnEnable()
        {
            _kart.OnHit += HandleHit;
            _kart.ItemBoxSystem.ItemActivated += ActivateItem;

            if (!_kart.IsPlayer)
                _kart.CheckpointCounter.LapFinished += FinishLap;
        }

        private void OnDisable()
        {
            _kart.OnHit -= HandleHit;
            _kart.ItemBoxSystem.ItemActivated -= ActivateItem;

            if (!_kart.IsPlayer)
                _kart.CheckpointCounter.LapFinished -= FinishLap;
        }

        private void FinishLap(int number)
        {
            if (_finished || !_kart.IsPlayer)
                return;

            if (number == _kart.CheckpointCounter.LapCount)
            {
                _finished = true;
                _finishAudioSource.Play();
            }
            else
            {
                _lapFinishAudioSource.Play();
            }
        }

        private void ActivateItem(Item obj)
        {
            _itemAudioSource.Play();
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