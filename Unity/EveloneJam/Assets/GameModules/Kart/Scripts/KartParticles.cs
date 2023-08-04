﻿using UnityEngine;


namespace Project.Kart
{
    public class KartParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _dustParticles;
        [SerializeField] private ParticleSystem[] _boostParticles;
        [SerializeField] private ParticleSystem[] _hitParticles;
        [SerializeField] private float _maxEmission = 70f;
        [SerializeField] private float _maxBoostEmission = 70f;
        private KartController _kart;

        private void Awake()
        {
            _kart = GetComponentInParent<KartController>();
        }

        private void OnEnable()
        {
            _kart.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            _kart.OnHit -= HandleHit;
        }

        private void Update ()
        {
            float dustParticleEmission = (_kart.IsGrounded && _kart.InDrift)
                ? _maxEmission : 0;

            for (int i = 0; i < _dustParticles.Length; i++)
            {
                var emission = _dustParticles[i].emission;
                emission.rateOverTime = dustParticleEmission;
            }

            float boostParticleEmission = (_kart.InBoost) ? _maxBoostEmission : 0;

            for (int i = 0; i < _boostParticles.Length; i++)
            {
                var emission = _boostParticles[i].emission;
                emission.rateOverTime = boostParticleEmission;
            }
        }

        private void HandleHit()
        {
            for (int i = 0; i < _hitParticles.Length; i++)
            {
                _hitParticles[i].Play();
            }
        }
    }
}