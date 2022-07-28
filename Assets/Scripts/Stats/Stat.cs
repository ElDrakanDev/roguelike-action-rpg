using System.Collections.Generic;
using System;
using Game.ID;

namespace Game.Stats
{
    public class Stat
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
                if (_value < min) return min;
                else if (_value > max) return max;
                return _value;
            }
            set { _value = value; }
        }
        readonly List<StatModifier> stats = new List<StatModifier>();
        readonly float min;
        readonly float max;

        public Stat(float baseValue, float argMin = 1, float argMax = float.MaxValue)
        {
            stats.Add(new StatModifier(baseValue, this, this, StatType.Flat));
            min = argMin;
            max = argMax;
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
            return _value;
        }

        public StatModifier Add(StatModifier stat)
        {
            stats.Add(stat);
            return stat;
        }

        public void Remove(object source)
        {
            stats.RemoveAll((stat) => stat.Source == source);
            needUpdate = true;
        }
    }
}