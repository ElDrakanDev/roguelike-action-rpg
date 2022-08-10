using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;
using Game.Interfaces;
using Game.Helpers;
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

            //playerControls.Player.Movement.performed += controller.ReadMovement;
            //playerControls.Player.Movement.canceled += controller.ReadMovement;
            //playerControls.Player.Jump.started += controller.Jump;
        }
        private void OnEnable()
        {
            if (playerControls != null) playerControls.Enable();
        }
        private void OnDisable()
        {
            if(playerControls != null) playerControls.Disable();
        }
        private void Start()
        {
            IInteractable[] interactables = GameObject.FindGameObjectWithTag("Interactable")?.GetComponents<IInteractable>();
            if(interactables != null && interactables.Length > 0)
            {
                foreach (var interactable in interactables)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(controller.point != null && controller.size != null)
                Gizmos.DrawWireCube(controller.point, controller.size);
        }
        public void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.numpadPlusKey.wasPressedThisFrame) stats.Add(new StatModifier(0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);
            else if (keyboard.numpadMinusKey.wasPressedThisFrame) stats.Add(new StatModifier(-0.1f, this, stats[AttributeID.Agility], StatType.Flat), AttributeID.Agility);

            if (keyboard.escapeKey.wasPressedThisFrame) 
            {
                Debug.Log("Application Quit by user");
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
    }
}
