namespace Game.States
{
    public abstract class Context<T> where T : State
    {
        protected T _current = null;
        public T Current {get => _current; protected set => _current = value; }

        public Context(T state)
        {
            TransitionTo(state);
        }

        bool TransitionTo(T state)
        {
            if(_current != state)
            {
                _current = state;
                _current.Context = this as Context<State>;
                return true;
            }
            return false;
        }
    }
}
