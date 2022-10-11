using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;

namespace Game.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        public static List<Projectile> projectiles = new List<Projectile>();
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected bool _moveable = true;
        public bool Moveable { get => _moveable; }
        [SerializeField] protected bool _destructible = true;
        public bool Destructible { get => _destructible; }
        [SerializeField] GameObject _container;
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
            if (_container) Destroy(_container);
            else Destroy(gameObject);
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
        public virtual void Despawn()
        {
            onDespawn?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnEnterGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnEnterPlayer(collision);
            else if (collisionLayer == Layers.NPC) OnEnterNpc(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnStayGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnStayPlayer(collision);
            else if (collisionLayer == Layers.NPC) OnStayNpc(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnExitGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnExitPlayer(collision);
            else if (collisionLayer == Layers.NPC) OnExitNpc(collision);
        }

        #region Events
        public event Action onDestroy;
        public event Action<Collider2D> onEnterNpc;
        protected void OnEnterNpc(Collider2D collision) => onEnterNpc?.Invoke(collision);
        public event Action<Collider2D> onEnterGround;
        protected void OnEnterGround(Collider2D collision) => onEnterGround?.Invoke(collision);
        public event Action<Collider2D> onEnterPlayer;
        protected void OnEnterPlayer(Collider2D collision) => onEnterPlayer?.Invoke(collision);
        public event Action<Collider2D> onStayNpc;
        protected void OnStayNpc(Collider2D collision) => onStayNpc?.Invoke(collision);
        public event Action<Collider2D> onStayGround;
        protected void OnStayGround(Collider2D collision) => onStayGround?.Invoke(collision);
        public event Action<Collider2D> onStayPlayer;
        protected void OnStayPlayer(Collider2D collision) => onStayPlayer?.Invoke(collision);
        public event Action<Collider2D> onExitNpc;
        protected void OnExitNpc(Collider2D collision) => onExitNpc?.Invoke(collision);
        public event Action<Collider2D> onExitGround;
        protected void OnExitGround(Collider2D collision) => onExitGround?.Invoke(collision);
        public event Action<Collider2D> onExitPlayer;
        protected void OnExitPlayer(Collider2D collision) => onExitPlayer?.Invoke(collision);

        public event Action onLifeTimeEnd;
        protected void OnLifeTimeEnd() => onLifeTimeEnd?.Invoke();
        public event Action onDespawn;
        protected void OnDespawn() => onDespawn?.Invoke();
        #endregion
        public static void Clear()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Despawn();
            }
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Despawn();
            }
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Despawn();
            }
        }
    }
}

