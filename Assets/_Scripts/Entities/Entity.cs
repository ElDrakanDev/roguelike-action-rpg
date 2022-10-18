using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Utils;
using Game.Interfaces;
using System.Linq;
using Game.Players;

namespace Game.Entities
{
    
    [RequireComponent(typeof(Collider2D))]
    public class Entity : MonoBehaviour, IHittable
    {
        public static List<Entity> entities = new List<Entity>();
        public static Dictionary<Team, List<Entity>> entitiesByTeam = new Dictionary<Team, List<Entity>>()
        {
            {Team.Neutral, new List<Entity>() },
            {Team.Friendly, new List<Entity>()},
            {Team.Enemy, new List<Entity>() }
        };
        [SerializeField] Rigidbody2D rb;
        // [SerializeField] EntityDataSO data;
        [SerializeField] EntityStats _stats;
        public EntityStats Stats { get => _stats; protected set => _stats = value; }
        public bool spawned = false;
        public GameObject attackTarget;
        public Vector3 attackTargetPos;
        public Vector3 moveTarget;
        public float Health
        { 
            get => _stats.Health;
            set {
                _stats.Health = value;
                if (_stats.Health <= 0) Death();
            } 
        }
        public Team Team { get => _stats.team; set { _stats.team = value; UpdateEntitiesLeft(); } }

        #region Creation
        public static Entity Create(EntityDataSO data, Vector3 position, Transform parent = null)
        {
            if (!parent) parent = Generation.Room.ActiveRoom.transform;
            var instance = Instantiate(data.Prefab, position, Quaternion.identity, parent);
            var entity = instance.GetComponent<Entity>();
            entity.Stats = data.Stats.CreateStats();
            return entity;
        }
        public static Entity Create(EntityDataSO data, Vector3 position, Team team, Transform parent = null)
        {
            if (!parent) parent = Generation.Room.ActiveRoom.transform;
            var instance = Instantiate(data.Prefab, position, Quaternion.identity, parent);
            var entity = instance.GetComponent<Entity>();
            entity.Stats = data.Stats.CreateStats(team);
            return entity;
        }
        #endregion
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
        public event Action<Entity> onDeath;
        protected void OnDeath() => onDeath?.Invoke(this);
        public event Action<Collider2D> onExitEntity;
        protected void OnExitEntity(Collider2D collision) => onExitEntity?.Invoke(collision);
        public event Action<Collider2D> onExitGround;
        protected void OnExitGround(Collider2D collision) => onExitGround?.Invoke(collision);
        public event Action<Collider2D> onExitPlayer;
        protected void OnExitPlayer(Collider2D collision) => onExitPlayer?.Invoke(collision);
        #endregion
        #region Instance Methods
        private void OnEnable()
        {
            entities.Add(this);
            entitiesByTeam[Team].Add(this);
            UpdateEntitiesLeft();
        }
        private void OnDisable()
        {
            entities.Remove(this);
            entitiesByTeam[Team].Remove(this);
            UpdateEntitiesLeft();
        }
        public virtual void Despawn()
        {
            Destroy(gameObject);
        }
        public virtual void Death()
        {
            onDeath?.Invoke(this);
            Destroy(gameObject);
        }
        private void Update() => AIUpdate();
        private void FixedUpdate() => AIFixedUpdate();
        protected virtual void AIUpdate() { }
        protected virtual void AIFixedUpdate() { }
        public virtual void SetAttackTarget(GameObject target) { attackTarget = target; if(target != null) attackTargetPos = target.transform.position; }
        protected virtual void UpdateAttackTarget()
        {
            List<GameObject> objects = new();
            if(Team == Team.Friendly)
            {
                objects.AddRange(entitiesByTeam[Team.Enemy].Select(entity => entity.gameObject));
                objects.AddRange(entitiesByTeam[Team.Neutral].Select(entity => entity.gameObject));
            }
            else if(Team == Team.Neutral)
            {
                objects.AddRange(entitiesByTeam[Team.Enemy].Select(entity => entity.gameObject));
                objects.AddRange(entitiesByTeam[Team.Friendly].Select(entity => entity.gameObject));
                objects.AddRange(entitiesByTeam[Team.Neutral].Select(entity => entity.gameObject));
                objects.AddRange(Player.players.Select(player => player.gameObject));
            }
            else if(Team == Team.Enemy)
            {
                objects.AddRange(entitiesByTeam[Team.Friendly].Select(entity => entity.gameObject));
                objects.AddRange(entitiesByTeam[Team.Neutral].Select(entity => entity.gameObject));
                objects.AddRange(Player.players.Select(player => player.gameObject));
            }
            objects.Remove(gameObject);
            SetAttackTarget(TargetChoose(objects));
        }
        protected virtual GameObject TargetChoose(IEnumerable<GameObject> objects) => objects.Closest(transform.position);
        public float Hit(float damage)
        {
            Health -= damage;
            return damage;
        }
        public float Hit(float damage, Vector2 direction, float knockback)
        {
            Health -= damage;
            if (rb)
            {
                rb.AddForce(direction * knockback, ForceMode2D.Impulse);
            }
            return damage;
        }
        #endregion
        #region Static Methods
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
        public static void UpdateEntitiesLeft()
        {
            Run.Run.instance.navigator.enemiesLeft = entitiesByTeam[Team.Enemy].Count > 0 || entitiesByTeam[Team.Neutral].Count > 0;
        }
        #endregion
    }
}
