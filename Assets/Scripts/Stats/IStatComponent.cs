using Game.ID;

namespace Game.Stats
{
    public interface IStatComponent
    {
        public float Value { get; set; }
        public StatType Type { get; set; }
        public Stat Owner { get; set; }
        public object Source { get; set; }
    }
}