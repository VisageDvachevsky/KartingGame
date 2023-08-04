using Project.Kart;
using Project.RoadGeneration;
using Project.StateMachines;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class RacingState : BaseState {

            private readonly InputLock _inputLock;
            private readonly PlayerKartProvider _kartProvider;
            private CheckpointCounter _checkpointCounter;

            public RacingState(InputLock inputLock, PlayerKartProvider kartProvider)
            {
                _inputLock = inputLock;
                _kartProvider = kartProvider;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                _inputLock.Unlock();
                _checkpointCounter = _kartProvider.Kart.CheckpointCounter;
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                if (_checkpointCounter.Finished)
                {
                    TransitTo<FinishState>();
                }
            }
        }
    }
}