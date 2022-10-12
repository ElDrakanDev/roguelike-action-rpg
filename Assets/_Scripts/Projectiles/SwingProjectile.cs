using UnityEngine;
using Game.Utils;

namespace Game.Projectiles
{
    public class SwingProjectile : Projectile
    {
        public override Vector3 Velocity { get; set; }

        protected override void Initialize(object owner, ProjectileDataSO data, float damage, Team state, Vector2 velocity)
        {
            this.owner = owner;
            Stats = data.Stats.CreateStats(damage, state);
            Stats.lifeTime = float.MaxValue;
            Velocity = velocity;
        }
    }
}
