namespace Game.Interfaces
{
    public interface IUsable : IUpdateable
    {
        public bool Usable { get; }
        public bool Use();
    }
}
