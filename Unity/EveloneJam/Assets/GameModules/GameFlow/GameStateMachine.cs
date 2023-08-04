using Project.Camera;
using Project.Kart;
using Project.RoadGeneration;
using Project.StateMachines;
using Project.UI;
using System;
using UnityEngine;
using Zenject;

namespace Project.GameFlow
{
    public partial class GameStateMachine : StateMachineBehavior
    {
        [SerializeField] private RoadBuilder _roadBuilder;
        [SerializeField] private KartSpawner _kartSpawner;
        [SerializeField] private CountdownDisplay _countdownDisplay;
        [SerializeField] private FinishMenu _finishMenu;
        [SerializeField] private InputLock _inputLock;

        private ICamera _camera;
        private PlayerKartProvider _kartProvider;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider, ICamera camera)
        {
            _camera = camera;
            _kartProvider = kartProvider;
        }

        protected override Type InitialState => typeof(MapCreationState);

        protected override void RegisterStates()
        {
            RegisterState(new MapCreationState(_roadBuilder, _kartSpawner, _camera, _kartProvider, _inputLock));
            RegisterState(new CountdownState(_countdownDisplay));
            RegisterState(new RacingState(_inputLock, _kartProvider));
            RegisterState(new FinishState(_finishMenu));
        }
    }
}