using Cinemachine;
using UnityEngine;

namespace Project.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineCamera : MonoBehaviour, ICamera
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        public float FOV {
            get => _camera.m_Lens.FieldOfView;
            set => _camera.m_Lens.FieldOfView = value;
        }

        public void Follow(Transform target)
        {
            _camera.Follow = target;
            _camera.LookAt = target;
        }

        public void Shake()
        {
        }
    }
}