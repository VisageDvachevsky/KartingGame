using Project.Interaction;
using Project.RoadGeneration;
using UnityEngine;

namespace Project.Kart
{
    [RequireComponent(typeof(Rigidbody))]
    public class KartTrigger : MonoBehaviour
    {
        public KartController Kart { get; set; }
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleTrigger(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }


        private void HandleTrigger(Collider other)
        {
            if (other.TryGetComponent(out IPickup obj))
            {
                if (!obj.PlayerOnly || Kart.IsPlayer)
                {
                    obj.Pickup(Kart);
                }
            }

            if (other.TryGetComponent<RoadCheckpoint>(out var checkpoint))
            {
                Kart.CheckpointCounter.ReachCheckpoint(checkpoint.Index);
            }
        }

        private void HandleCollision(Collision collision)
        {
            if (collision.transform.TryGetComponent(out KartTrigger _))
            {
                Kart.CollideWithOtherKart(collision.contacts[0].normal, collision.relativeVelocity);
            }

            if (collision.transform.TryGetComponent(out RoadGuard _))
            {
                Kart.CollideWithRoadGuard(collision.contacts[0].normal, collision.relativeVelocity);
            }
        }
    }
}