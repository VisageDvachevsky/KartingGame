using System;
using UnityEngine;

namespace Project.StateMachines
{
    public class TestStateMachine : StateMachineBehavior
    {
        private class FooState : BaseState
        {
            public override void OnRegistered(StateMachineBehavior stateMachine)
            {
                base.OnRegistered(stateMachine);
                Debug.Log($"Foo registered in {stateMachine}");
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Debug.Log("Foo entered");
            }

            public override void OnExit()
            {
                base.OnExit();
                Debug.Log("Foo exited");
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                Debug.Log("Foo ticked");

                if (Input.GetKeyDown(KeyCode.Q)) TransitTo<BarState>();
            }

            public override void OnPause()
            {
                base.OnPause();
                Debug.Log("Foo paused");
            }

            public override void OnResume()
            {
                base.OnPause();
                Debug.Log("Foo resumed");
            }
        }

        private class BarState : BaseState
        {
            public override void OnRegistered(StateMachineBehavior stateMachine)
            {
                base.OnRegistered(stateMachine);
                Debug.Log($"Bar registered in {stateMachine}");
            }

            public override void OnEnter()
            {
                base.OnEnter();
                Debug.Log("Bar entered");
            }

            public override void OnExit()
            {
                base.OnExit();
                Debug.Log("Bar exited");
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                Debug.Log("Bar ticked");

                if (Input.GetKeyDown(KeyCode.W)) TransitTo<FooState>();
            }

            public override void OnPause()
            {
                base.OnPause();
                Debug.Log("Bar paused");
            }

            public override void OnResume()
            {
                base.OnPause();
                Debug.Log("Bar resumed");
            }
        }

        protected override Type InitialState => typeof(FooState);

        protected override void RegisterStates()
        {
            RegisterState(new FooState());
            RegisterState(new BarState());
        }
    }
}
