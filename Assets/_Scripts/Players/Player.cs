using UnityEngine;
using Game.Stats;
using Game.Input;
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
        PlayerController controller;
        PlayerActionsControls playerControls;
        Team _;
        public Team Team { get => Team.Friendly; set => _ = value; }
        [SerializeField] float _health;

        private void Awake()
        {
            stats = new CharacterStats(this, baseStats.baseStats, () => Destroy(gameObject));
            controller = new PlayerController(null, this, gameObject);
            playerControls = new PlayerActionsControls();
        }
        private void OnEnable()
        {
            playerControls?.Enable();
            players.Add(this);
        }
        private void OnDisable()
        {
            playerControls?.Disable();
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

            controller.Update();
        }
        private void FixedUpdate()
        {
            controller.FixedUpdate();
        }
        private void OnDestroy()
        {
            EventManager.OnPlayerDespawn(gameObject);
        }
        public void Move(InputAction.CallbackContext context) => controller.Move(context);
        public void Jump(InputAction.CallbackContext context) => controller.Jump(context);
        public void MoveSkill(InputAction.CallbackContext context) => controller.Dash(context);
        public void Interact(InputAction.CallbackContext context) => controller.Interact(context);
        public void MainAttack(InputAction.CallbackContext context) => controller.MainAttack(context);

        public float Hit(float damage)
        {
            return stats.Hit(damage);
        }
        public float Hit(float damage, Vector2 direction, float knockback)
        {
            return stats.Hit(damage, direction, knockback);
        }
    }
}
