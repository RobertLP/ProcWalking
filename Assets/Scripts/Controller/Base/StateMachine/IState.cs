//Author: Arjan Cordia (arjaneend@gmail.com) licensed to Robert Popijus
using System;

namespace StateMachine
{
    public delegate IState<T> StateFinished<T>(IState<T> next);
    public delegate IState<T> StateFinishedType<T>(Type next);

    public interface IState
    {
        void Initialize();
        void Exit();
        void OnUpdate();
        void OnFixedUpdate();
		void OnLateUpdate();
    }

    public interface IState<T> : IState
    {
        void RegisterState(T entity);
        IState<T> DispatchFinishEvent(IState<T> next);
        R DispatchFinishEvent<R>() where R : IState<T>;
        StateFinished<T> OnFinish
        {
            get;
            set;
        }
        StateFinishedType<T> OnFinishType
        {
            get;
            set;
        }
    }
}