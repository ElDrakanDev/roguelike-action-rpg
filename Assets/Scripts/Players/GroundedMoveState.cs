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

        public override void Move(Vector2 direction, float acceleration, float maxSpeed, float gravity, float maxFall)
        {
            float xSpeed = Mathf.Lerp(rb.velocity.x, direction.x * maxSpeed, acceleration);
            float extraFallSpeed = Mathf.Clamp(direction.y, -1, 0.3f);
            float ySpeed = rb.velocity.y - gravity + extraFallSpeed * gravity;
            ySpeed = ySpeed < maxFall ? ySpeed : maxFall;
            ySpeed = ySpeed > -maxFall ? ySpeed : -maxFall;

            rb.velocity = new Vector2(xSpeed, ySpeed);
        }
    }
}
