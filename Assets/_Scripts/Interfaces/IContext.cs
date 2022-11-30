namespace Game.Interfaces
{
    public interface IContext<T> where T : IState
    {
        public T Current { get; set; }

        protected bool TransitionTo(T state)
        {
            if(Current.Equals(state) is false)
            {
                Current = state;
                Current.Context = this as IContext<IState>;
                return true;
            }
            return false;
        }
    }
}
