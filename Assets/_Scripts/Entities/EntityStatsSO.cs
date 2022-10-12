using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;
using Game.Utils;

namespace Game.Entities
{
    [CreateAssetMenu(menuName = "Entities/Stats")]
    public class EntityStatsSO : ScriptableObject
    {
        public readonly float maxHealth;
        public float damage;
        public Team defaultTeam;
        public float baseSpeed;
        public float damageReduction;

        public EntityStats CreateStats()
        {
            return new EntityStats(maxHealth, damage, defaultTeam, baseSpeed, damageReduction);
        }
        public EntityStats CreateStats(Team team)
        {
            return new EntityStats(maxHealth, damage, team, baseSpeed, damageReduction);
        }
    }
    [System.Serializable]
    public class EntityStats
    {
        [SerializeField] float _health;
        public float Health { get { return _health; } set { _health = value; } }
        public readonly float _maxHealth;
        public float damage;
        public Team team;
        public float baseSpeed;
        public float damageReduction;

        public EntityStats(float maxHealth, float damage, Team team, float baseSpeed, float damageReduction)
        {
            _maxHealth = maxHealth;
            this.damage = damage;
            this.team = team;
            this.baseSpeed = baseSpeed;
            this.damageReduction = damageReduction;
        }
    }
}
