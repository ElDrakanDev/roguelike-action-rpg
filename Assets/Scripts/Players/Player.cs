using UnityEngine;
using Game.Stats;
using Game.Input;
using UnityEngine.InputSystem;

namespace Game.Players
{
    public class Player : MonoBehaviour
    {
        public CharacterStats stats;
        [SerializeField] BaseStatObject baseStats;
        [SerializeField] LayerMask ground;
        [SerializeField] PlayerInput input;
        PlayerController controller;
        PlayerActionsControls playerControls;

        private void Awake()
        {
            stats = new CharacterStats(this, baseStats.baseStats);
            controller = new PlayerController(null, this, gameObject, ground);
            playerControls = new PlayerActionsControls();
        }
        private void OnEnable()
        {
            if (playerControls != null) playerControls.Enable();
        }
        private void OnDisable()
        {
            if(playerControls != null) playerControls.Disable();
        }

        public void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.numpadPlusKey.wasPressedThisFrame) stats.Add(new StatModifier(0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);
            else if (keyboard.numpadMinusKey.wasPressedThisFrame) stats.Add(new StatModifier(-0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);

            if (keyboard.escapeKey.wasPressedThisFrame) 
            {
                Application.Quit();
            }
            controller.Update();
        }
        private void FixedUpdate()
        {
            controller.FixedUpdate();
        }
        public void PrintAttributes()
        {
            string msg = "";
            foreach (var attribute in stats.Attributes)
            {
                msg += $"{attribute}: {stats[attribute].Value}\n";
            }
            Debug.Log(msg);
        }

        public void Move(InputAction.CallbackContext context)
        {
            controller.ReadMovement(context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            controller.Jump(context);
        }

        public void MoveSkill(InputAction.CallbackContext context)
        {
            controller.MoveSkill(context);
        }

        public void Interact(InputAction.CallbackContext context)
        {
            PrintAttributes();
            controller.Interact(context);
        }
    }
}
