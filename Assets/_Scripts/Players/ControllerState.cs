using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;
using UnityEngine.InputSystem;
using Game.Stats;

namespace Game.Players
{
    public abstract class ControllerState : State
    {
        protected const float MAX_SPEED = 10f;

        protected readonly GameObject gameObject;
        protected readonly Rigidbody2D rb;
        protected readonly BoxCollider2D collider;
        protected readonly Player player;
        protected LayerMask ground;
        protected LayerMask platform;
        protected Vector2 inputDirection;
        protected Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }
        protected Vector3 Position { get => gameObject.transform.position; set => gameObject.transform.position = value; }
        public float Speed
        {
            get => Mathf.Clamp((player.stats[AttributeID.Agility].Value + 1) * 0.05f, 0.05f, 0.2f);
        }
        protected float MaxSpeed { get => Mathf.Clamp(MAX_SPEED + (player.stats[AttributeID.Agility].Value - 1), MAX_SPEED - 3, MAX_SPEED + 5); }
        public ControllerState(GameObject gameObject, Player player)
        {
            this.gameObject = gameObject;
            this.player = player;
            rb = gameObject.GetComponent<Rigidbody2D>();
            collider = gameObject.GetComponent<BoxCollider2D>();
            ground = LayerMask.GetMask("Ground");
            platform = LayerMask.GetMask("Platform");
        }
        public abstract void Update();
        public abstract void FixedUpdate();
        public void ReadMovement(InputAction.CallbackContext context) => inputDirection = context.ReadValue<Vector2>();
        public abstract void Jump(InputAction.CallbackContext context);
        public abstract void Dash(InputAction.CallbackContext context);
    }
}
