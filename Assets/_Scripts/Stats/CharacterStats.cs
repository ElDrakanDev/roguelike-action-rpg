using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class CharacterStats
    {
        public readonly Dictionary<AttributeID, Stat> statsDict = new Dictionary<AttributeID, Stat>();
        public Dictionary<AttributeID, Stat>.KeyCollection Attributes { get => statsDict.Keys; }
        public Dictionary<AttributeID, Stat>.ValueCollection Stats { get => statsDict.Values; }

        public Stat this[AttributeID attribute]
        {
            get 
            {
                if (statsDict.ContainsKey(attribute))
                    return statsDict[attribute];
                Debug.LogWarning($"Entity stats contains no attribute {attribute}. Returned null.");
                return null;
            }
            set
            {
                if (statsDict.ContainsKey(attribute))
                {
                    statsDict[attribute] = value;
                    return;
                }
                Debug.LogWarning($"Couldn't set inexistent attribute {attribute} to {value}.");
            }
        }

        public CharacterStats(dynamic owner, BaseStatValue[] baseStats)
        {
            foreach(var baseStat in baseStats)
            {
                statsDict.Add(
                    baseStat.attribute,
                    new Stat(baseStat.baseValue, owner, baseStat.minValue, baseStat.maxValue));
            }
        }

        public void Add(StatModifier modifier, AttributeID attribute)
        {
            this[attribute].Add(modifier);
        }
        public void Remove(dynamic source)
        {
            foreach (var stat in Stats) 
            { 
                stat.RemoveFromSource(source);
            }
        }
    }
}
