using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        public static List<Projectile> projectiles = new List<Projectile>();
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected bool _moveable = true;
        public bool Moveable { get => _moveable; }
        public object owner;
        [SerializeField] ProjectileStats _stats;
        public ProjectileStats Stats { get => _stats; protected set => _stats = value; }
        public Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }

        #region Creation

        public static Projectile Create(
            object owner, ProjectileDataSO data, float damage, ProjectileState state, Vector2 position, Vector2 velocity, float rotation = 0
        )
        {
            Quaternion quat = Quaternion.Euler(0, 0, rotation);
            GameObject gO = Instantiate(data.Prefab, position, quat);
            Projectile projectile = GetProjectileComponent(gO);
            projectile.Initialize(owner, data, damage, state, velocity);

            return projectile;
        }
        public static Projectile Create(
            object owner, ProjectileDataSO data, float damage, ProjectileState state, Vector2 position, Vector2 velocity, Quaternion rotation
        )
        {
            GameObject gO = Instantiate(data.Prefab, position, rotation);
            Projectile projectile = GetProjectileComponent(gO);
            projectile.Initialize(owner, data, damage, state, velocity);

            return projectile;
        }

        static Projectile GetProjectileComponent(GameObject gO)
        {
            if (gO.TryGetComponent(out Projectile projectile))
                return projectile;
            return gO.GetComponentInChildren<Projectile>();
        }
        protected virtual void Initialize(object owner, ProjectileDataSO data, float damage, ProjectileState state, Vector2 velocity)
        {
            this.owner = owner;
            Stats = data.Stats.CreateStats(damage, state);
            Velocity = velocity;
        }

        #endregion

        private void OnEnable()
        {
            projectiles.Add(this);
        }
        private void OnDisable()
        {
            projectiles.Remove(this);
        }
        private void OnDestroy()
        {
            projectiles.Remove(this);
            onDestroy?.Invoke();
        }
        private void Update()
        {
            Stats.lifeTime -= Time.deltaTime;
            if (Stats.lifeTime <= 0) LifeTimeEnd();
            AIUpdate();
        }
        private void FixedUpdate() => AIFixedUpdate();
        public void LifeTimeEnd()
        {
            onLifeTimeEnd?.Invoke();
            Destroy(gameObject);
        }
        protected virtual void AIUpdate() { }
        protected virtual void AIFixedUpdate() { }

        #region Events
        public event Action onDestroy;
        public event Action onHit;
        protected void OnHit() => onHit?.Invoke();
        public event Action onLifeTimeEnd;
        protected void OnLifeTimeEnd() => onLifeTimeEnd?.Invoke();
        #endregion
    }
}

