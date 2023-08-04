using Project.Camera;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSmooth : MonoBehaviour, ICamera
{
    [SerializeField] public float _smoothTime = 0.5f;
    [SerializeField] public Vector3 Offset;
    [SerializeField] public Transform Target;

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
        transform.position += Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, _smoothTime) - transform.position;

        transform.LookAt(Target);
    }

    public void Follow(Transform target)
    {
        Target = target;
        Vector3 desiredPosition = Target.TransformPoint(Offset);
        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
