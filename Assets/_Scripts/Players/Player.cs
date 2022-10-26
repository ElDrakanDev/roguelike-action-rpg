using UnityEngine;
using Game.Stats;
using UnityEngine.InputSystem;
using Game.Events;
using Game.Interfaces;
using Game.Utils;
using System.Collections.Generic;

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
            stats = new CharacterStats(this, baseStats.baseStats, () => Destroy(gameObject));
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
            //Keyboard keyboard = Keyboard.current;
            //if (keyboard.numpadPlusKey.wasPressedThisFrame) stats.Add(new StatModifier(0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);
            //else if (keyboard.numpadMinusKey.wasPressedThisFrame) stats.Add(new StatModifier(-0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);
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
