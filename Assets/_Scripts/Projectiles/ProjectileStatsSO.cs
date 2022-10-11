using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public enum ProjectileState { Friendly, Neutral, Enemy}
    [CreateAssetMenu(menuName = "Projectiles/Stats")]
    public class ProjectileStatsSO : ScriptableObject
    {
        public float speed;
        public float lifeTime;

        public ProjectileStats CreateStats(float damage, ProjectileState state)
        {
            return new ProjectileStats(damage, lifeTime, speed, state);
        }
    }

    [System.Serializable]
    public class ProjectileStats
    {
        public float damage;
        public float lifeTime;
        public float speed;
        public ProjectileState state;

        public ProjectileStats(float damage, float lifeTime, float speed, ProjectileState state)
        {
            this.damage = damage;
            this.lifeTime = lifeTime;
            this.speed = speed;
            this.state = state;
        }
    }
}
