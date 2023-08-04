using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;
using Project.Camera;

namespace Project.Kart
{
    public class BoostEffects : MonoBehaviour
    {
        [SerializeField] private float _boostFOV = 90f;
        [SerializeField] private Volume _postProcessing;
        [SerializeField] private float _boostChromaticAberration = 0.75f;

        private float _defaultFOV;
        private float _defaultChromaticAberration;

        private PlayerKartProvider _kartProvider;
        private ICamera _camera;

        [Inject]
        private void Construct(PlayerKartProvider playerKartProvider, ICamera camera)
        {
            _kartProvider = playerKartProvider;
            _camera = camera;
        }

        private void Start()
        {
            _defaultFOV = _camera.FOV;
            if (_postProcessing.profile.TryGet<ChromaticAberration>(out var effect))
            {
                _defaultChromaticAberration = effect.intensity.value;
            }
        }

        private void Update()
        {
            KartController kart = _kartProvider.Kart;
            if (kart == null) return;

            float targetFOV = kart.InBoost ? _boostFOV : _defaultFOV;
            float targetAberration = kart.InBoost ? _boostChromaticAberration : _defaultChromaticAberration;

            _camera.FOV = Mathf.Lerp(_camera.FOV, targetFOV, Time.deltaTime * 10f);

            if (_postProcessing.profile.TryGet<ChromaticAberration>(out var effect))
            {
                effect.intensity.value = Mathf.Lerp(effect.intensity.value, targetAberration, Time.deltaTime * 10f);
            }
        }
    }
}
