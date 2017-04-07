using System;

namespace VillagePeople.StateMachine
{
    public class StateMachine<T>
    {
        public State<T> PrevState;
        public State<T> CurrentState;
        private readonly T _entity;

        public StateMachine(T entity)
        {
            _entity = entity;
        }

        public void ChangeState(State<T> newState)
        {
            PrevState = CurrentState;
            PrevState?.Exit(_entity);
            CurrentState = newState;
            CurrentState.Enter(_entity);
        }

        public void Update()
        {
            if (PrevState == null)
            {
                CurrentState.Enter(_entity);
                PrevState = CurrentState;
            }
            CurrentState?.Execute(_entity);
        }
    }
}
