using UnityEngine;
using Game.Utils;

namespace Game.Projectiles
{
    public class SwingProjectile : Projectile
    {
        protected override Vector3 _rbVelocity { get; set; }

        protected override void Initialize(GameObject owner, ProjectileDataSO data, float damage, Team state, Vector2 velocity, float knockback = 1)
        {
            this.owner = owner;
            Stats = data.Stats.CreateStats(damage, state, knockback);
            Stats.lifeTime = float.MaxValue;
            Stats.bounces = int.MaxValue;
            Velocity = velocity;
        }
    }
}
