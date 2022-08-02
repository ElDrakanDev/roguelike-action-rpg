namespace Game.States
{
    public abstract class State
    {
        protected Context _context = null;
        public Context Context { get => _context; set => _context = value; }
    }
}
