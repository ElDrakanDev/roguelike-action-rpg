using UnityEngine;

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
                if (direction.x == 0) acceleration *= 1.5f;
                float xSpeed = Mathf.Lerp(rb.velocity.x, direction.x * maxSpeed, acceleration);
                float extraFallSpeed = Mathf.Clamp(direction.y * 1.5f, -1.5f, 0.3f);
                if (extraFallSpeed < 0) maxFall *= 1.5f;
                float ySpeed = rb.velocity.y - gravity + extraFallSpeed * gravity;
                ySpeed = ySpeed < maxFall ? ySpeed : maxFall;
                ySpeed = ySpeed > -maxFall ? ySpeed : -maxFall;

                rb.velocity = new Vector2(xSpeed, ySpeed);
            }
        }

        public override void MoveSkill(Vector2 direction, float speed, float maxSpeed)
        {
            if(direction.x > 0.8f || direction.x < -0.8f)
            rb.velocity = new Vector2(direction.x * maxSpeed * 2.5f, 0);

            dashFrames = 5;
        }
    }
}
