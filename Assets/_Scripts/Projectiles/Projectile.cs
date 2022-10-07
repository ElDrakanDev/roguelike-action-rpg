using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        public object owner;
        [SerializeField] ProjectileStats _stats;
        public ProjectileStats Stats { get => _stats; protected set => _stats = value; }
        public Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }

        private void Update()
        {
            Stats.lifeTime -= Time.deltaTime;
            if (Stats.lifeTime <= 0) Destroy(gameObject);
        }

        #region Creation

        public static Projectile Create(
            object owner, ProjectileDataSO data, float damage, ProjectileStates state, Vector2 position, Vector2 velocity, float rotation = 0
        )
        {
            Quaternion quat = Quaternion.Euler(0, 0, rotation);
            GameObject gO = Instantiate(data.Prefab, position, quat);
            Projectile projectile = gO.GetComponent<Projectile>();
            projectile.Stats = data.Stats.Stats(damage, state);
            projectile.owner = owner;
            projectile.Velocity = velocity * projectile.Stats.speed;

            return projectile;
        }
        public static Projectile Create(
            object owner, ProjectileDataSO data, float damage, ProjectileStates state, Vector2 position, Vector2 velocity, Quaternion rotation
        )
        {
            GameObject gO = Instantiate(data.Prefab, position, rotation);
            Projectile projectile = gO.GetComponent<Projectile>();
            projectile.Stats = data.Stats.Stats(damage, state);
            projectile.owner = owner;
            projectile.Velocity = velocity * projectile.Stats.speed;

            return projectile;
        }

        #endregion
    }
}

