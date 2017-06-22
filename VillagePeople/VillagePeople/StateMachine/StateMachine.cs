namespace VillagePeople.StateMachine {
    public class StateMachine<T> {
        private readonly T _entity;
        public State<T> CurrentState;
        public State<T> PrevState;

        public StateMachine(T entity) {
            _entity = entity;
        }

        public void ChangeState(State<T> newState) {
            PrevState = CurrentState;
            PrevState?.Exit(_entity);
            CurrentState = newState;
            CurrentState.Enter(_entity);
        }

        public void Update() {
            if (PrevState == null) {
                CurrentState.Enter(_entity);
                PrevState = CurrentState;
            }
            CurrentState?.Execute(_entity);
        }
    }
}