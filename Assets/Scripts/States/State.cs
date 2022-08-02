namespace Game.States
{
    public abstract class State
    {
        protected Context<State> _context = null;
        public Context<State> Context { get => _context; set => _context = value; }
    }
}
