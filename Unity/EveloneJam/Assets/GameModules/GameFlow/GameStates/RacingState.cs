using Project.Kart;
using Project.RoadGeneration;
using Project.StateMachines;
using Project.UI;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class RacingState : BaseState {

            private readonly InputLock _inputLock;
            private readonly PlayerKartProvider _kartProvider;
            private readonly HUD _hud;
            private readonly Tutorial _tutorial;
            private CheckpointCounter _checkpointCounter;

            public RacingState(InputLock inputLock, PlayerKartProvider kartProvider, HUD hud, Tutorial tutorial)
            {
                _inputLock = inputLock;
                _kartProvider = kartProvider;
                _hud = hud;
                _tutorial = tutorial;
            }

            public override void OnEnter()
            {
                base.OnEnter();

                _inputLock.Unlock();
                _hud.Show();
                _checkpointCounter = _kartProvider.Kart.CheckpointCounter;
                _tutorial.StartTutorial();
            }

            public override void OnExit()
            {
                base.OnExit();

                _hud.Hide();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                if (_checkpointCounter.Finished)
                    TransitTo<FinishState>();
            }
        }
    }
}