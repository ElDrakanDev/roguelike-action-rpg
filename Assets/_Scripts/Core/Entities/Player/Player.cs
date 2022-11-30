using UnityEngine;
using Game.Stats;
using UnityEngine.InputSystem;
using Game.Events;
using Game.Interfaces;
using Game.Utils;
using System.Collections.Generic;
using System;

namespace Game.Players
{
    public class Player : BaseEntity, IHittable
    {
        public static List<Player> players = new();
        public CharacterStats stats;
        public IWeapon weapon;
        Team _;
        new public Team Team { get => Team.Friendly; set => _ = value; }
        public float MaxHealth { get => stats.MaxHealth; }
        public float Health { get => stats.Health; set => stats.Health = value; }
        SpriteAnimator animator;
        [SerializeField] BaseStatObject _baseStats;
        [SerializeField] SpriteAnimationSO hurtAnimation;
        [SerializeField] PlayerGhost ghost;
        private void Awake()
        {
            animator = GetComponent<SpriteAnimator>();
            stats = new CharacterStats(this, _baseStats.baseStats, OnDeath);
        }
        private void OnEnable()
        {
            players.Add(this);
            EventManager.OnPlayerSpawn(gameObject);
        }
        private void OnDisable()
        {
            players.Remove(this);
            EventManager.OnPlayerDespawn(gameObject);
        }

        public void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.numpadPlusKey.wasPressedThisFrame)
                foreach(var attribute in Enum.GetValues(typeof(AttributeID)))
                    stats.Add(new StatModifier(0.1f, this, stats[(AttributeID)attribute], StatType.Flat), (AttributeID)attribute);
            else if (keyboard.numpadMinusKey.wasPressedThisFrame)
                foreach (var attribute in Enum.GetValues(typeof(AttributeID)))
                    stats.Add(new StatModifier(-0.1f, this, stats[(AttributeID)attribute], StatType.Flat), (AttributeID)attribute);
        }
        void OnDeath()
        {
            foreach (var controller in GetComponents<PlayerController>()) controller.enabled = false;
            animator.EndAllAnimations();
            EventManager.OnPlayerDeath(gameObject);
            //instantiate corpse
            ghost.enabled = true;
            this.enabled = false;
            
        }
        public float Hit(float damage)
        {
            animator.SetAnimation(hurtAnimation, 1, true);
            return stats.Hit(damage);
        }
        public float Hit(float damage, Vector2 direction, float knockback)
        {
            animator.SetAnimation(hurtAnimation, 1, true);
            return stats.Hit(damage, direction, knockback);
        }
    }
}
