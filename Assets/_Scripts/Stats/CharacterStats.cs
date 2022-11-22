using Game.Interfaces;
using Game.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class CharacterStats : IHittable
    {
        public readonly Dictionary<AttributeID, Stat> statsDict = new Dictionary<AttributeID, Stat>();
        public Dictionary<AttributeID, Stat>.KeyCollection Attributes { get => statsDict.Keys; }
        public Dictionary<AttributeID, Stat>.ValueCollection Stats { get => statsDict.Values; }
        event Action onHealth0;

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

        float _health;
        public float Health 
        { 
            get => _health; 
            set 
            {
                if (value <= 0)
                {
                    _health = 0;
                    onHealth0?.Invoke();
                } 
                else if (value > MaxHealth) _health = MaxHealth;
                else _health = value;
            } 
        }

        public float MaxHealth { get => statsDict[AttributeID.MaxHealth].Value; }
        public Team Team { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CharacterStats(dynamic owner, BaseStatValue[] baseStats, Action onHealth0 = null)
        {
            foreach(var baseStat in baseStats)
            {
                statsDict.Add(
                    baseStat.attribute,
                    new Stat(baseStat.baseValue, owner, baseStat.minValue, baseStat.maxValue));
            }
            _health = statsDict[AttributeID.MaxHealth].Value;
            this.onHealth0 += onHealth0;
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

        public float Hit(float damage)
        {
            Health -= damage;
            return damage;
        }

        public float Hit(float damage, Vector2 direction, float knockback) => Hit(damage);

        public float StatTotal(AttributeID attribute) => statsDict[attribute].Value;
        public float StatTotal(IEnumerable<AttributeID> attributes)
        {
            float total = 0;
            foreach (var attribute in attributes)
            {
                total += statsDict[attribute].Value;
            }
            return total;
        }

        public float StatTotal(StatType type, AttributeID attribute)
        {
            float total = 0;
            foreach(var stat in statsDict[attribute][type])
            {
                total += stat.Value;
            }
            return total;
        }
        public float StatTotal(StatType type, IEnumerable<AttributeID> attributes)
        {
            float total = 0;
            foreach(var attribute in attributes)
            {
                foreach (var stat in statsDict[attribute][type])
                {
                    total += stat.Value;
                }
            }
            return total;
        }
    }
}
