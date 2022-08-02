using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.States;

namespace Game.Players
{
    public class PlayerController : Context<ControllerState>
    {
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
            this.gameObject = gameObject;
            this.player = player;
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction, float speed, float maxSpeed, float gravity)
        {
            Current.Move(direction, speed, maxSpeed, gravity);
        }

        public void Jump(float force)
        {
            if (isGrounded)
                Current.Jump(force);
        }

        public void Update() { }
    }
}
