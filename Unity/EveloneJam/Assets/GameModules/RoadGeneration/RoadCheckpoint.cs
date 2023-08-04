using UnityEngine;

namespace Project.RoadGeneration
{
    [RequireComponent(typeof(Collider))]
    public class RoadCheckpoint : MonoBehaviour
    {
        private Collider _collider;

        public int Index { get; private set; }
        public Vector3 WorldPosition => transform.position;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        public void Init(int index)
        {
            Index = index;
        }

        public Vector3 GetNearestPoint(Vector3 to)
        {
            return _collider.ClosestPoint(to);
        }
    }
}