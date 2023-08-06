using Project.Camera;
using Project.Interaction;
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
        [SerializeField] private TerrainGenerator _terrainGenerator;
        [SerializeField] private KartSpawner _kartSpawner;
        [SerializeField] private CountdownDisplay _countdownDisplay;
        [SerializeField] private HUD _hud;
        [SerializeField] private FinishMenu _finishMenu;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private InputLock _inputLock;

        private ICamera _camera;
        private PlayerKartProvider _kartProvider;
        private CoinSystem _coinSystem;
        private GlobalMoney _globalMoney;

        [Inject]
        private void Construct(PlayerKartProvider kartProvider, ICamera camera, GlobalMoney globalMoney, CoinSystem coinSystem)
        {
            _camera = camera;
            _kartProvider = kartProvider;
            _coinSystem = coinSystem;
            _globalMoney = globalMoney;
        }

        protected override Type InitialState => typeof(MapCreationState);

        protected override void RegisterStates()
        {
            RegisterState(new MapCreationState(_roadBuilder, _terrainGenerator, _kartSpawner, _camera, _kartProvider, _inputLock));
            RegisterState(new CountdownState(_countdownDisplay));
            RegisterState(new RacingState(_inputLock, _kartProvider, _hud, _tutorial));
            RegisterState(new FinishState(_finishMenu, _globalMoney, _coinSystem));
        }
    }
}