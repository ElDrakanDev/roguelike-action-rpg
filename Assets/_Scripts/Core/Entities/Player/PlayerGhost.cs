using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Players
{
    public class PlayerGhost : MonoBehaviour
    {
        const float VELOCITY_LERP_SPEED = 0.9f;
        [SerializeField] float _speed = 10f;
        [SerializeField] Material ghostMaterial;
        Material _initialMaterial;
        PlayerInput _input;
        Rigidbody2D rb;
        SpriteRenderer spriteRenderer;
        Player player;
        bool facingRightOnStart;
        bool FacingRight { get => facingRightOnStart && !spriteRenderer.flipX; }
        Vector2 direction;
        Dictionary<string, Action<InputAction.CallbackContext>> inputHandlers;
        #region Setup
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInput>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            player = GetComponent<Player>();
            inputHandlers = new()
            {
                {"Movement", Move },
                {"Jump", IgnoreAction },
                {"MoveSkill", IgnoreAction },
                {"Interact", IgnoreAction },
                {"MainAttack", IgnoreAction }
            };
        }
        private void OnEnable()
        {
            facingRightOnStart = transform.localScale.x >= 0;
            _input.onActionTriggered += HandleInput;
            _initialMaterial = spriteRenderer.material;
            spriteRenderer.material = ghostMaterial;
        }
        private void OnDisable() => _input.onActionTriggered -= HandleInput;
        #endregion
        private void Update()
        {
            FaceDirection();
        }
        private void FixedUpdate()
        {
            rb.velocity = Vector3.Lerp(rb.velocity, direction * _speed, VELOCITY_LERP_SPEED);
        }
        void HandleInput(InputAction.CallbackContext context)
        {
            if (inputHandlers.TryGetValue(context.action.name, out var handler))
                handler(context);
        }
        void FaceDirection()
        {
            if (
                (direction.x > 0 && FacingRight is false) ||
                (direction.x < 0 && FacingRight is true)
            )
                spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        void Move(InputAction.CallbackContext context) => direction = context.ReadValue<Vector2>();
        void IgnoreAction(InputAction.CallbackContext _) { }
        public void Revive()
        {
            // TODO: Reset position to door entrance or other closest player
            foreach (var controller in GetComponents<PlayerController>()) controller.enabled = true;
            player.enabled = true;
            player.Health = player.MaxHealth;
            spriteRenderer.material = _initialMaterial;
            this.enabled = false;
        }
    }
}
