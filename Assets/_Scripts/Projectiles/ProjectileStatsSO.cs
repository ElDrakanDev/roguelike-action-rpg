using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;

namespace Game.Projectiles
{
    [CreateAssetMenu(menuName = "Projectiles/Stats")]
    public class ProjectileStatsSO : ScriptableObject
    {
        public float speed;
        public float lifeTime;
        public float knockback = 1;

        public ProjectileStats CreateStats(float damage, Team team)
        {
            return new ProjectileStats(damage, lifeTime, speed, team, knockback);
        }
    }

    [System.Serializable]
    public class ProjectileStats
    {
        public float damage;
        public float lifeTime;
        public float speed;
        public Team team;
        public float knockback = 1;
        public ProjectileStats(float damage, float lifeTime, float speed, Team team, float knockback)
        {
            this.damage = damage;
            this.lifeTime = lifeTime;
            this.speed = speed;
            this.team = team;
            this.knockback = knockback;
        }
    }
}
