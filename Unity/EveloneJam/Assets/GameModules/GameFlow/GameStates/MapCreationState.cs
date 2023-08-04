using Project.Camera;
using Project.Kart;
using Project.RoadGeneration;
using Project.StateMachines;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class MapCreationState : BaseState
        {
            private readonly RoadBuilder _roadBuilder;
            private readonly KartSpawner _kartSpawner;
            private readonly ICamera _camera;
            private readonly PlayerKartProvider _kartProvider;
            private readonly InputLock _inputLock;

            public MapCreationState(
                RoadBuilder roadBuilder,
                KartSpawner kartSpawner,
                ICamera camera,
                PlayerKartProvider kartProvider,
                InputLock inputLock)
            {
                _roadBuilder = roadBuilder;
                _kartSpawner = kartSpawner;
                _camera = camera;
                _kartProvider = kartProvider;
                _inputLock = inputLock;
            }

            public override void OnEnter()
            {
                base.OnEnter();

                _roadBuilder.CreateNewRoad();
                _inputLock.Lock();
                _kartSpawner.RespawnKart();
                _camera.Follow(_kartProvider.Kart.transform);

                TransitTo<CountdownState>();
            }
        }
    }
}