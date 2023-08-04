using Project.RoadGeneration;
using System;
using UnityEngine;
using Zenject;

namespace Project.Kart
{
    [RequireComponent(typeof(KartController))]
    public class PlayerKart : MonoBehaviour
    {
        [SerializeField] private AIKart _ai;

        private IKartInput _kartInput;
        private KartController _kart;
        private CheckpointCounter _checkpointCounter;

        [Inject]
        private void Construct(IKartInput kartInput)
        {
            _kartInput = kartInput;
        }

        private void Awake()
        {
            _kart = GetComponent<KartController>();
            _checkpointCounter = _kart.CheckpointCounter;
            _kart.SetInput(_kartInput);
        }

        private void OnEnable()
        {
            _checkpointCounter.AllLapsFinished += SetAutopilot;
        }

        private void OnDisable()
        {
            _checkpointCounter.AllLapsFinished -= SetAutopilot;
        }

        private void SetAutopilot()
        {
            Debug.Log("autopilot");
            _kart.SetInput(_ai);
        }
    }
}