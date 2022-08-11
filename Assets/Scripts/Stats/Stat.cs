using System.Collections.Generic;
using System;
using UnityEngine;

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

        public readonly List<IStatComponent> stats = new List<IStatComponent>();
        readonly float min;
        readonly float max;
        public readonly dynamic owner;

        public Stat(float baseValue, dynamic argOwner, float argMin = 1, float argMax = float.MaxValue, StatType type = StatType.Flat)
        {
            stats.Add(new StatModifier(baseValue, this, this, StatType.Flat));
            min = argMin;
            max = argMax;
            owner = argOwner;
            Type = type;
        }

        private float UpdateValue()
        {
            float flatStats = 0;
            float multStats = 1;

            for (int i = 0; i < stats.Count; i++)
            {
                if (stats[i].Type == StatType.Flat)
                    flatStats += stats[i].Value;
                else if (stats[i].Type == StatType.Mult)
                    multStats += stats[i].Value;
            }

            needUpdate = false;
            _value = (float)Math.Round(flatStats * multStats, 2);
            if (StatOwner != null) StatOwner.needUpdate = true;
            return _value;
        }

        public void Add(IStatComponent stat)
        {
            stats.Add(stat);
            needUpdate = true;
        }

        public bool RemoveComponent(IStatComponent component)
        {
            return stats.Remove(component);
        }

        public void RemoveFromSource(dynamic source)
        {
            stats.RemoveAll((stat) => {
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
            needUpdate = true;
        }
    }
}