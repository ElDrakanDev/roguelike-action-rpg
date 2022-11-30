using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Players
{
    public class GroundedMoveState : ControllerState
    {
        float _maxDashCooldown => 1f;
        float _dashLength => 0.1f;
        float _maxFall => -20f * _timeScale;
        float _gravity => Mathf.Pow(_timeScale, 2);
        float dashSeconds = 0;
        float dashCooldown = 0;
        float _jumpForce => -_maxFall;
        bool Grounded
        {
            get
            {
                Vector2 point = new Vector2(rb.position.x, rb.position.y - boxCollider.bounds.size.y * 0.5f);
                Vector2 size = new Vector2(boxCollider.bounds.size.x * 0.95f, boxCollider.bounds.size.y * 0.1f);
                return Physics2D.OverlapBox(point, size, 0, groundLayer) != null;
            }
        }

        protected override void Update()
        {
            base.Update();
            dashSeconds -= Time.deltaTime * _timeScale;
            dashCooldown -= Time.deltaTime * _timeScale;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(dashSeconds < 0)
            {
                float acceleration = Speed / (_timeScale > 0 ? _timeScale : 0.01f);
                float maxFall = _maxFall;

                if (inputDirection.x == 0) acceleration *= 1.5f;
                float xSpeed = Mathf.Lerp(Velocity.x, inputDirection.x * _maxSpeed, acceleration);
                float extraFallSpeed = Mathf.Clamp(inputDirection.y * 1.5f, -1.5f, 0.3f) * _timeScale;
                if (extraFallSpeed < 0) maxFall *= 1.5f;
                float ySpeed = Velocity.y - _gravity + extraFallSpeed * _gravity;
                ySpeed = Mathf.Clamp(ySpeed, maxFall, -maxFall);

                Velocity = new Vector2(xSpeed, ySpeed);
            }
        }
        public override void Jump(InputAction.CallbackContext context)
        {
            if(Grounded && context.started)
                Velocity = new Vector2(Velocity.x, _jumpForce);
            print($"Intended jump speed: {_jumpForce}");
        }
        public override void Dash(InputAction.CallbackContext context)
        {
            if (!context.started || dashCooldown > 0) return;
            dashCooldown = _maxDashCooldown;
            if(inputDirection.x > 0.8f || inputDirection.x < -0.8f)
            Velocity = new Vector2(inputDirection.x * _maxSpeed * 2.5f, 0);

            dashSeconds = _dashLength;
        }
    }
}
