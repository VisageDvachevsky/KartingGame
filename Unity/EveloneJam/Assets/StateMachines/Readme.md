# Пример конечного автомата
```C#
public class TestStateMachine : StateMachineBehavior
    {
        private class FooState : BaseState
        {
            // Вызывается в момент регистрации состояния в конечном автомате
            public override void OnRegistered(StateMachineBehavior stateMachine)
            {
                base.OnRegistered(stateMachine);
                Debug.Log($"Foo registered in {stateMachine}");
            }

            // Вызывается, когда конечный автомат переходит в это состояние
            public override void OnEnter()
            {
                base.OnEnter();
                Debug.Log("Foo entered");
            }

            // Вызывается, когда конечный автомат выходит из этого состояния
            public override void OnExit()
            {
                base.OnExit();
                Debug.Log("Foo exited");
            }

            // Вызывается каждый кадр, когда конечный автомат обновляет состояние
            public override void OnUpdate()
            {
                base.OnUpdate();
                Debug.Log("Foo ticked");

                if (Input.GetKeyDown(KeyCode.Q)) TransitTo<BarState>();
            }

            // Вызывается, когда конечный автомат ставится на паузу с этим состоянием или перед выходом из состояния
            public override void OnPause()
            {
                base.OnPause();
                Debug.Log("Foo paused");
            }

            // Вызывается, когда конечный автомат снимается с паузы с этим состоянием или после входа в состояние
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

        // Состояние, в которое конечный автомат входит в начале
        protected override Type InitialState => typeof(FooState);

        // Регистрация состояний в конечном автомате
        protected override void RegisterStates()
        {
            RegisterState(new FooState());
            RegisterState(new BarState());
        }
    }
```