using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Players
{
    public class GroundedMoveState : ControllerState
    {
        Rigidbody2D rb;
        public GroundedMoveState(Rigidbody2D rb)
        {
            this.rb = rb;
        }

        public override void Jump(float force)
        {
            rb.velocity = new Vector2(rb.velocity.x, force);
        }

        public override void Move(Vector2 direction, float speed, float maxSpeed, float gravity)
        {
            Vector2 vel = new Vector2(, rb.velocity.y);
        }
    }
}
