namespace Project.StateMachines
{
    public abstract class BaseState
    {
        protected StateMachineBehavior StateMachine { get; private set; }
        protected bool Paused { get; private set; }

        public void TransitTo<T>() where T : BaseState
        {
            StateMachine.TransitTo<T>();
        }

        /// <summary>
        /// Вызывается в момент регистрации состояния в конечном автомате
        /// </summary>
        public virtual void OnRegistered(StateMachineBehavior stateMachine)
        {
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Вызывается, когда конечный автомат переходит в это состояние
        /// </summary>
        public virtual void OnEnter() { }
        /// <summary>
        /// Вызывается, когда конечный автомат выходит из этого состояния
        /// </summary>
        public virtual void OnExit() { }
        /// <summary>
        /// Вызывается каждый кадр, когда конечный автомат обновляет состояние
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// Вызывается, когда конечный автомат ставится на паузу с этим состоянием или перед выходом из состояния
        /// </summary>
        public virtual void OnPause() {
            Paused = true;
        }
        /// <summary>
        /// Вызывается, когда конечный автомат снимается с паузы с этим состоянием или после входа в состояние
        /// </summary>
        public virtual void OnResume() {
            Paused = false;
        }
    }
}
