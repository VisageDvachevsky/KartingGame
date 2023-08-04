using Project.StateMachines;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class FinishState : BaseState
        {
            private readonly FinishMenu _finishMenu;

            public FinishState(FinishMenu finishMenu)
            {
                _finishMenu = finishMenu;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                _finishMenu.Show();
            }
        }
    }
}