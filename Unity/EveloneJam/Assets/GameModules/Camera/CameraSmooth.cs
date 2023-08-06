using DG.Tweening;
using Project.Camera;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSmooth : MonoBehaviour, ICamera
{
    [SerializeField] public float SmoothTime = 0.5f;
    [SerializeField] public Vector3 Offset;
    [SerializeField] public Transform Target;
    [SerializeField] public float ShakeDuration = 0.5f;
    [SerializeField] public float ShakeStrength = 1f;
    [SerializeField] public float RotationShakeStrength = 1f;

    private Camera _camera;
    private Vector3 velocity = Vector3.zero;

    public float FOV
    {
        get => _camera.fieldOfView;
        set => _camera.fieldOfView = value;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 desiredPosition = Target.TransformPoint(Offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, SmoothTime);

        transform.LookAt(Target);
    }

    public void Follow(Transform target)
    {
        Target = target;
        Vector3 desiredPosition = Target.TransformPoint(Offset);
        transform.position = desiredPosition;
        transform.LookAt(target);
    }

    public void Shake()
    {
        transform.DOComplete();
        transform.DOShakePosition(ShakeDuration, ShakeStrength);
        transform.DOShakeRotation(ShakeDuration, RotationShakeStrength);
    }
}
