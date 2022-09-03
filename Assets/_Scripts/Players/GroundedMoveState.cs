using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Players
{
    public class GroundedMoveState : ControllerState
    {
        const float MAX_DASH_COOLDOWN = 1f;
        const float MAX_FALL = -20f;
        const float GRAVITY = 1f;
        float dashSeconds = 0;
        bool Grounded
        {
            get
            {
                Vector2 point = new Vector2(rb.position.x, rb.position.y - collider.bounds.size.y * 0.5f);
                Vector2 size = new Vector2(collider.bounds.size.x * 0.99f, collider.bounds.size.y * 0.1f);
                return Physics2D.OverlapBox(point, size, 0, groundLayer) != null;
            }
        }

        public override void Update()
        {
            dashSeconds -= Time.deltaTime;
        }
        public override void FixedUpdate()
        {
            if(dashSeconds < 0)
            {
                float acceleration = Speed;
                float maxFall = MAX_FALL;

                if (inputDirection.x == 0) acceleration *= 1.5f;
                float xSpeed = Mathf.Lerp(rb.velocity.x, inputDirection.x * MAX_SPEED, acceleration);
                float extraFallSpeed = Mathf.Clamp(inputDirection.y * 1.5f, -1.5f, 0.3f);
                if (extraFallSpeed < 0) maxFall *= 1.5f;
                float ySpeed = rb.velocity.y - GRAVITY + extraFallSpeed * GRAVITY;
                if (ySpeed < maxFall) ySpeed = maxFall;
                else if (ySpeed > -maxFall) ySpeed = -maxFall;

                Velocity = new Vector2(xSpeed, ySpeed);
            }
        }
        public GroundedMoveState(GameObject gameObject, Player player) : base(gameObject, player) {}

        public override void Jump(InputAction.CallbackContext context)
        {
            if(Grounded && context.started)
                rb.velocity = new Vector2(rb.velocity.x, -MAX_FALL);
        }
        public override void Dash(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            dashSeconds = MAX_DASH_COOLDOWN;
            if(inputDirection.x > 0.8f || inputDirection.x < -0.8f)
            rb.velocity = new Vector2(inputDirection.x * MaxSpeed * 2.5f, 0);

            dashSeconds = 0.1f;
        }
    }
}
