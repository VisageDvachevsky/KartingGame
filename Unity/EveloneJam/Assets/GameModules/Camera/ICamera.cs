using UnityEngine;

namespace Project.Camera
{
    public interface ICamera
    {
        float FOV { get; set; }
        void Follow(Transform target);
    }
}