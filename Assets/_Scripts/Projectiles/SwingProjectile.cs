using UnityEngine;

namespace Game.Projectiles
{
    public class SwingProjectile : Projectile
    {
        [SerializeField] bool hasPivot = true;
        Vector2 _;
        new public Vector2 Velocity { get => Vector2.zero; set => _ = value; }
        protected override void Initialize(object owner, ProjectileDataSO data, float damage, ProjectileState state, Vector2 velocity)
        {
            this.owner = owner;
            Stats = data.Stats.CreateStats(damage, state);
            Stats.lifeTime = float.MaxValue;
        }

        new public void LifeTimeEnd()
        {
            OnLifeTimeEnd();
            if (hasPivot) Destroy(transform.parent);
            else Destroy(gameObject);
        }
    }
}
