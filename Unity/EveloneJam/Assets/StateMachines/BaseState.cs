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
        /// ���������� � ������ ����������� ��������� � �������� ��������
        /// </summary>
        public virtual void OnRegistered(StateMachineBehavior stateMachine)
        {
            StateMachine = stateMachine;
        }

        /// <summary>
        /// ����������, ����� �������� ������� ��������� � ��� ���������
        /// </summary>
        public virtual void OnEnter() { }
        /// <summary>
        /// ����������, ����� �������� ������� ������� �� ����� ���������
        /// </summary>
        public virtual void OnExit() { }
        /// <summary>
        /// ���������� ������ ����, ����� �������� ������� ��������� ���������
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// ����������, ����� �������� ������� �������� �� ����� � ���� ���������� ��� ����� ������� �� ���������
        /// </summary>
        public virtual void OnPause() {
            Paused = true;
        }
        /// <summary>
        /// ����������, ����� �������� ������� ��������� � ����� � ���� ���������� ��� ����� ����� � ���������
        /// </summary>
        public virtual void OnResume() {
            Paused = false;
        }
    }
}
