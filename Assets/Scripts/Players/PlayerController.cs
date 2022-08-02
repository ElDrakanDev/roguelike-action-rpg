using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;
using Game.Stats;


namespace Game.Players
{
    public class PlayerController : Context<ControllerState>
    {
        const float MAX_FALL = 20f;
        const float MAX_SPEED = 30f;
        const float GRAVITY = 1f;
        PlayerActionsControls inputs;
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
        readonly GameObject gameObject;
        readonly Player player;
        Rigidbody2D rb;
        bool isGrounded;

        public PlayerController(ControllerState state, Player player, GameObject gameObject) : base(state) 
        {
            inputs = new PlayerActionsControls();
            this.gameObject = gameObject;
            this.player = player;
            rb = gameObject.GetComponent<Rigidbody2D>();
            if (state == null)
                Current = new GroundedMoveState(rb);
        }

        public void Move(Vector2 direction)
        {
            float speed = Mathf.Clamp(player.stats[AttributeID.Agility].Value * 0.4f, 0.55f, 1);
            Current.Move(direction, speed, MAX_SPEED, GRAVITY, MAX_FALL);
        }

        public void Jump(float force)
        {
            if (isGrounded)
                Current.Jump(force);
        }

        public void Update() 
        {

        }
    }
}
