namespace Game.Interfaces
{
    public interface IState
    {
        public IContext<IState> Context { get; set; }
    }
}