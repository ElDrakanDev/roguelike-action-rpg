using System.Collections.Generic;
using System;
using System.Linq;

namespace Game.Stats
{
    public class Stat : IStatComponent
    {
        public bool needUpdate = true;
        private float _value;
        public float Value
        {
            get
            {
                if (!needUpdate)
                {
                    if (_value < min) return min;
                    else if (_value > max) return max;
                    return _value;
                }
                UpdateValue();
                return _value;
            }
            set
            {
                if (value < min) _value = min;
                else if (value > max) _value = max;
                else _value = value;
            }
        }

        public StatType Type { get; set; }
        public Stat StatOwner { get; set; }
        public object Source { get; set; }

        public readonly Dictionary<StatType, List<IStatComponent>> stats = new() {
            { StatType.Flat, new List<IStatComponent>()},
            {StatType.Mult, new List<IStatComponent>()},
        };
        readonly float min;
        readonly float max;
        public readonly dynamic owner;

        public Stat(float baseValue, dynamic argOwner, float argMin = 1, float argMax = float.MaxValue, StatType type = StatType.Flat)
        {
            stats[StatType.Flat].Add(new StatModifier(baseValue, this, this, StatType.Flat));
            min = argMin;
            max = argMax;
            owner = argOwner;
            Type = type;
        }

        private float UpdateValue()
        {
            float flatStats = 0;
            float multStats = 1;

            foreach(var modifier in stats[StatType.Flat]) flatStats += modifier.Value;
            foreach (var modifier in stats[StatType.Mult]) multStats += modifier.Value;

            needUpdate = false;
            _value = (float)Math.Round(flatStats * multStats, 2);
            if (StatOwner != null) StatOwner.needUpdate = true;
            return _value;
        }

        public void Add(IStatComponent stat)
        {
            stats[stat.Type].Add(stat);
            needUpdate = true;
        }

        public bool RemoveComponent(IStatComponent component)
        {
            return stats[component.Type].Remove(component);
        }

        public void RemoveFromSource(dynamic source)
        {
            foreach(var statList in stats.Values.ToList())
            {
                statList.RemoveAll((stat) => {
                    try
                    {
                        bool remove = stat.Source == source;
                        return remove;
                    }
                    catch
                    {
                        return false;
                    }
                });
            }
            needUpdate = true;
        }

        public List<IStatComponent> this[StatType type]
        {
            get => stats[type];
        }
    }
}