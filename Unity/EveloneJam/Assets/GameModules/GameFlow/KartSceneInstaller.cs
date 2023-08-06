using Project.Camera;
using Project.GameFlow;
using Project.Interaction;
using Project.Kart;
using Project.Laps;
using Project.RoadGeneration;
using UnityEngine;
using Zenject;

namespace Project
{
    public class KartSceneInstaller : MonoInstaller
    {
        [SerializeField] private MapSettings _settings;
        [SerializeField] private InputLock _inputLock;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private RoadBuilder _roadBuilder;
        [SerializeField] private ScoreSystem _scoreSystem;
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private CameraSmooth _camera;

        public override void InstallBindings()
        {
            Container.Bind<MapSettings>().ToSelf().FromInstance(_settings);
            Container.Bind<ScoreSystem>().ToSelf().FromInstance(_scoreSystem);
            Container.Bind<ItemConfig>().ToSelf().FromInstance(_itemConfig);
            Container.Bind<IInputLock>().To<InputLock>().FromInstance(_inputLock);
            Container.Bind<IKartInput>().To<PlayerInput>().FromInstance(_playerInput);
            Container.Bind<ICamera>().To<CameraSmooth>().FromInstance(_camera);
            Container.Bind<IRoadProvider>().To<RoadBuilder>().FromInstance(_roadBuilder);
            Container.Bind<PlayerKartProvider>().ToSelf().FromNew().AsSingle();
            Container.Bind<CoinSystem>().ToSelf().FromNew().AsSingle();
            Container.Bind<PauseController>().ToSelf().FromNew().AsSingle();
        }
    }
}