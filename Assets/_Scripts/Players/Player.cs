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
    public class Player : MonoBehaviour, IHittable
    {
        public static List<Player> players = new();
        public CharacterStats stats;
        public IWeapon weapon;
        [SerializeField] BaseStatObject baseStats;
        [SerializeField] PlayerInput input;
        Team _;
        public Team Team { get => Team.Friendly; set => _ = value; }
        [SerializeField] float _health;
        SpriteAnimator animator;
        [SerializeField] SpriteAnimationSO hurtAnimation;
        private void Awake()
        {
            animator = GetComponent<SpriteAnimator>();
            stats = new CharacterStats(this, baseStats.baseStats, () => {
                if(players.Count == 1) EventManager.OnPlayerLose();
                Destroy(gameObject); 
            });
        }
        private void OnEnable()
        {
            players.Add(this);
        }
        private void OnDisable()
        {
            players.Remove(this);
        }

        private void Start()
        {
            EventManager.OnPlayerSpawn(gameObject);
        }
        public void Update()
        {
            _health = stats.Health;
            Keyboard keyboard = Keyboard.current;
            if (keyboard.numpadPlusKey.wasPressedThisFrame)
                foreach(var attribute in Enum.GetValues(typeof(AttributeID)))
                    stats.Add(new StatModifier(0.1f, this, stats[(AttributeID)attribute], StatType.Flat), (AttributeID)attribute);
            else if (keyboard.numpadMinusKey.wasPressedThisFrame)
                foreach (var attribute in Enum.GetValues(typeof(AttributeID)))
                    stats.Add(new StatModifier(-0.1f, this, stats[(AttributeID)attribute], StatType.Flat), (AttributeID)attribute);
        }

        private void OnDestroy()
        {
            EventManager.OnPlayerDespawn(gameObject);
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
