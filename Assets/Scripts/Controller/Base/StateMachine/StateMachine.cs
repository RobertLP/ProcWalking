//Author: Arjan Cordia (arjaneend@gmail.com) licensed to Robert Popijus
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace StateMachine
{
    public class StateMachine<T>
    {
        private T owner;

        private IState<T> currentState;
        private IState<T> previousState;

        public Action<IState<T>> StateChanged;

		private Dictionary<Type, IState<T>> stateDictionary;

        private bool stateInitialized = false;

        public System.Type currentType
        {
            get { return currentState.GetType(); }
        }

        public StateMachine(T owner)
        {
			this.stateDictionary = new Dictionary<Type, IState<T>>();
            this.owner = owner;
            currentState = null;
            previousState = null;
        }

		public void RegisterState(IState<T> state)
		{
			Type stateType = state.GetType();
			stateDictionary.Add(stateType, state);
		}

		public void RegisterState(params IState<T>[] States) // Addition by robert
		{
			int l = States.Length;
			for(int i = 0; i < l; i++)
			{
				Type stateType = States[i].GetType();
				stateDictionary.Add(stateType, States[i]);
			}
		}

        public void Update()
        {
            if (!stateInitialized && currentState != null)
            {
                stateInitialized = true;
                currentState.Initialize();
            }
            if (currentState != null)
                currentState.OnUpdate();
        }

		public void FixedUpdate()
		{
			if(currentState != null)
				currentState.OnFixedUpdate();
		}

		public void LateUpdate()
		{
			if(currentState != null)
				currentState.OnLateUpdate();
		}

		public R ChangeState<R>() where R : IState<T>
		{
			Type type = typeof(R);
			if(stateDictionary.ContainsKey(type))
				return (R)ChangeState(stateDictionary[type]);

			return default(R);
		}

        public R GetState<R>() where R : IState<T>
        {
            Type type = typeof(R);
			if(stateDictionary.ContainsKey(type))
				return (R)stateDictionary[type];

			return default(R);
        }

		public IState<T> ChangeState(Type type)
		{
			if(stateDictionary.ContainsKey(type))
				return ChangeState(stateDictionary[type]);

			return null;
		}

        public IState<T> ChangeState(IState<T> newState)
        {
            previousState = currentState;

            if (currentState != null)
            {
                currentState.OnFinish -= ChangeState;
                currentState.OnFinishType -= ChangeState;
                currentState.Exit();
            }

            currentState = newState;
            if(newState != null)
            {
                newState.OnFinish += ChangeState;
                newState.OnFinishType += ChangeState;
                newState.RegisterState(owner);
            }
            stateInitialized = false;

            if(StateChanged != null)
                StateChanged(newState);

            return newState;
        }

        public void RevertToPreviousState()
        {
            if (previousState != null)
                ChangeState(previousState);
        }
    }
}
