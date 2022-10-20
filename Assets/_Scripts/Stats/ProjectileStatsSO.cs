using UnityEngine;
using Game.Utils;

namespace Game.Stats
{
    [CreateAssetMenu(menuName = "Projectiles/Stats")]
    public class ProjectileStatsSO : ScriptableObject
    {
        public float speed = 5;
        public float lifeTime = 3;
        public int pierces = 1;
        public int bounces = 1;

        public ProjectileStats CreateStats(float damage, Team team, float knockback)
        {
            return new ProjectileStats(damage, lifeTime, speed, team, knockback, pierces, bounces);
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
        public int pierces = 1;
        public int bounces = 1;
        public ProjectileStats(float damage, float lifeTime, float speed, Team team, float knockback, int pierces, int bounces)
        {
            this.damage = damage;
            this.lifeTime = lifeTime;
            this.speed = speed;
            this.team = team;
            this.knockback = knockback;
            this.pierces = pierces;
            this.bounces = bounces;
        }
    }
}
