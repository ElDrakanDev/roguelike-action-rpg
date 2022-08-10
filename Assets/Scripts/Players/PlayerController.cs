using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;
using Game.Stats;
using Game.Input;
using UnityEngine.InputSystem;

namespace Game.Players
{
    public class PlayerController : Context<ControllerState>
    {
        const float MAX_FALL = 20f;
        const float MAX_SPEED = 10f;
        const float GRAVITY = 1f;
        const float MAX_MOVE_SKILL_COOLDOWN = 1f;
        public float Speed
        {
            get => Mathf.Clamp(player.stats[AttributeID.Agility].Value * 0.1f, 0.05f, 1);
        }
        float MaxSpeed { get => Mathf.Clamp(MAX_SPEED + (player.stats[AttributeID.Agility].Value - 1), MAX_SPEED - 3, MAX_SPEED + 3); }
        Vector2 direction;
        readonly GameObject gameObject;
        readonly Player player;
        Rigidbody2D rb;
        bool isGrounded;
        BoxCollider2D collider;
        LayerMask groundLayer;
        public Vector2 point, size;
        float _moveSkillCooldown = 0f;


        public new ControllerState Current 
        { 
            get 
            {
                if(_current is null)
                    _current = new GroundedMoveState(rb);
                return _current;
            }
            protected set => _current = value;
        }

        public PlayerController(ControllerState state, Player player, GameObject gameObject, LayerMask groundLayer) : base(state) 
        {
            this.gameObject = gameObject;
            this.player = player;
            this.groundLayer = groundLayer;
            rb = gameObject.GetComponent<Rigidbody2D>();
            collider = gameObject.GetComponent<BoxCollider2D>();
            if (state == null)
                Current = new GroundedMoveState(rb);
        }

        public void Move()
        {
            Current.Move(direction, Speed, MaxSpeed, GRAVITY, MAX_FALL);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (isGrounded) Current.Jump(MAX_FALL);
        }

        public void ReadMovement(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>();
        }
        public void Update()
        {
            _moveSkillCooldown -= Time.deltaTime;
        }
        public void FixedUpdate() 
        {
            Move();
            isGrounded = CheckGrounded();
        }

        public void MoveSkill(InputAction.CallbackContext context)
        {
            if(_moveSkillCooldown < 0)
            {
                Current.MoveSkill(direction, Speed, MaxSpeed);
                _moveSkillCooldown = MAX_MOVE_SKILL_COOLDOWN;
            }
        }

        bool CheckGrounded()
        {
            point = new Vector2(rb.position.x, rb.position.y - collider.bounds.size.y * 0.5f);
            size = new Vector2(collider.bounds.size.x * 0.99f, collider.bounds.size.y * 0.1f);
            return Physics2D.OverlapBox(point, size, 0, groundLayer) != null;
        }
    }
}
