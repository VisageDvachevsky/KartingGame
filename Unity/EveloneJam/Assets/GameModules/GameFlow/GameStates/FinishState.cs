using Project.Interaction;
using Project.StateMachines;

namespace Project.GameFlow
{
    public partial class GameStateMachine
    {
        private class FinishState : BaseState
        {
            private readonly FinishMenu _finishMenu;
            private readonly CoinSystem _coinSystem;
            private readonly GlobalMoney _globalMoney;

            public FinishState(FinishMenu finishMenu, GlobalMoney globalMoney, CoinSystem coinSystem)
            {
                _finishMenu = finishMenu;
                _coinSystem = coinSystem;
                _globalMoney = globalMoney;
            }

            public override void OnEnter()
            {
                base.OnEnter();

                _globalMoney.AddMoney(_coinSystem.CoinsAmount);
                _finishMenu.Show();
            }
        }
    }
}