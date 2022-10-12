using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using Game.Interfaces;

namespace Game.Entities
{
    
    [RequireComponent(typeof(Collider2D))]
    public class Entity : MonoBehaviour, IHittable
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] EntityDataSO data;
        [SerializeField] EntityStats _stats;
        public static List<Entity> entities = new List<Entity>();
        public EntityStats Stats { get => _stats; protected set => _stats = value; }
        public Team Team { get => _stats.team; set => _stats.team = value; }

        #region Creation
        public static Entity Create(EntityDataSO data, Vector3 position)
        {
            var instance = Instantiate(data.Prefab, position, Quaternion.identity);
            return instance.GetComponent<Entity>();
        }
        #endregion
        private void OnEnable()
        {
            entities.Add(this);
        }
        private void OnDisable()
        {
            entities.Remove(this);
        }
        public virtual void Despawn()
        {
            Destroy(gameObject);
        }
        private void Update() => AIUpdate();
        private void FixedUpdate() => AIFixedUpdate();
        protected virtual void AIUpdate() { }
        protected virtual void AIFixedUpdate() { }

        #region Collision
        private void OnTriggerEnter2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnEnterGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnEnterPlayer(collision);
            else if (collisionLayer == Layers.ENTITY) OnEnterEntity(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnStayGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnStayPlayer(collision);
            else if (collisionLayer == Layers.ENTITY) OnStayEntity(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;
            if (collisionLayer == Layers.GROUND) OnExitGround(collision);
            else if (collisionLayer == Layers.PLAYER) OnExitPlayer(collision);
            else if (collisionLayer == Layers.ENTITY) OnExitEntity(collision);
        }
        #endregion

        #region Events
        public event Action<Collider2D> onEnterEntity;
        protected void OnEnterEntity(Collider2D collision) => onEnterEntity?.Invoke(collision);
        public event Action<Collider2D> onEnterGround;
        protected void OnEnterGround(Collider2D collision) => onEnterGround?.Invoke(collision);
        public event Action<Collider2D> onEnterPlayer;
        protected void OnEnterPlayer(Collider2D collision) => onEnterPlayer?.Invoke(collision);
        public event Action<Collider2D> onStayEntity;
        protected void OnStayEntity(Collider2D collision) => onStayEntity?.Invoke(collision);
        public event Action<Collider2D> onStayGround;
        protected void OnStayGround(Collider2D collision) => onStayGround?.Invoke(collision);
        public event Action<Collider2D> onStayPlayer;
        protected void OnStayPlayer(Collider2D collision) => onStayPlayer?.Invoke(collision);
        public event Action<Collider2D> onExitEntity;
        protected void OnExitEntity(Collider2D collision) => onExitEntity?.Invoke(collision);
        public event Action<Collider2D> onExitGround;
        protected void OnExitGround(Collider2D collision) => onExitGround?.Invoke(collision);
        public event Action<Collider2D> onExitPlayer;
        protected void OnExitPlayer(Collider2D collision) => onExitPlayer?.Invoke(collision);
        #endregion
        public static void Clear()
        {
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Despawn();
            }
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Despawn();
            }
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                entities[i].Despawn();
            }
        }

        public float Hit(float damage)
        {
            Stats.Health -= damage;
            return damage;
        }
        public float Hit(float damage, Vector2 direction, float knockback)
        {
            Stats.Health -= damage;
            if (rb)
            {
                rb.AddForce(direction * knockback, ForceMode2D.Impulse);
            }
            return damage;
        }
    }
}
