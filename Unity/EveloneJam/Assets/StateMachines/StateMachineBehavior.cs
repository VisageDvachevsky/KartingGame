using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.StateMachines
{
    public abstract class StateMachineBehavior : MonoBehaviour
    {
        private Dictionary<Type, BaseState> _states = new Dictionary<Type, BaseState>();
        private BaseState _nextState = null;

        public BaseState CurrentState { get; private set; }
        protected virtual void OnEnable()
        {
            CurrentState?.OnResume();
        }

        protected virtual void OnDisable()
        {
            CurrentState?.OnPause();
        }

        protected virtual void Awake()
        {
            RegisterStates();
        }

        protected virtual void Start()
        {
            TransitTo(InitialState);
        }

        protected virtual void Update()
        {
            if (_nextState != null)
            {
                CurrentState = _nextState;
                _nextState = null;
                CurrentState.OnEnter();
                CurrentState.OnResume();
            }

            if (_nextState == null)
            {
                CurrentState.OnUpdate();
            }
        }

        public void TransitTo<T>() where T : BaseState
        {
            TransitTo(typeof(T));
        }

        public void TransitTo(Type type)
        {
            if (_nextState != null)
            {
                throw new InvalidOperationException($"This state machine is already in transition");
            }

            if (!_states.TryGetValue(type, out _nextState))
            {
                throw new ArgumentException($"This state machine doesn't contain {type.Name} state");
            }

            CurrentState?.OnPause();
            CurrentState?.OnExit();
        }

        protected void RegisterState<T>(T state) where T : BaseState
        {
            if (state == null)
            {
                throw new NullReferenceException($"{nameof(state)} was null");
            }

            _states[typeof(T)] = state;
            state.OnRegistered(this);
        }

        protected abstract Type InitialState { get; }
        protected abstract void RegisterStates();
    }
}
