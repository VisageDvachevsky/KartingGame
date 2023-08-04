using Project.StateMachines;
using Project.UI;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class CountdownState : BaseState
        {
            private CountdownDisplay _display;

            public CountdownState(CountdownDisplay display)
            {
                _display = display;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                _display.Activate();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (!_display.Active) TransitTo<RacingState>();
            }
        }
    }
}