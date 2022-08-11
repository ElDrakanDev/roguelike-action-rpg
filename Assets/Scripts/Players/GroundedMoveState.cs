using UnityEngine;
using System.Threading.Tasks;

namespace Game.Players
{
    public class GroundedMoveState : ControllerState
    {
        Rigidbody2D rb;
        int dashFrames = 0;
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
            dashFrames--;

            if(dashFrames < 0)
            {
                acceleration = direction.x == 0 ? acceleration * 1.5f : acceleration;
                float xSpeed = Mathf.Lerp(rb.velocity.x, direction.x * maxSpeed, acceleration);
                float extraFallSpeed = Mathf.Clamp(direction.y * 1.5f, -1.5f, 0.3f);
                float ySpeed = rb.velocity.y - gravity + extraFallSpeed * gravity;
                ySpeed = ySpeed < maxFall ? ySpeed : maxFall;
                ySpeed = ySpeed > -maxFall ? ySpeed : -maxFall;

                rb.velocity = new Vector2(xSpeed, ySpeed);
            }
        }

        public override void MoveSkill(Vector2 direction, float speed, float maxSpeed)
        {
            rb.velocity = new Vector2(direction.x * maxSpeed * 2.5f, 0);

            dashFrames = 5;
        }
    }
}
