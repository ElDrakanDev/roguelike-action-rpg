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
        protected LayerMask groundLayer;
        protected LayerMask platformLayer;
        protected Vector2 inputDirection;
        bool ignoringPlatforms = false;
        bool shouldIgnorePlatforms;
        int platformLayerValue;
        protected Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }
        protected Vector3 Position { get => gameObject.transform.position; set => gameObject.transform.position = value; }
        GameObject ActiveRoomGO { get => Run.Run.instance.ActiveRoom.gameObject; }
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
            groundLayer = LayerMask.GetMask("Ground", "Platform");
            platformLayer = LayerMask.GetMask("Platform");
            platformLayerValue = LayerMask.NameToLayer("Platform");
        }
        public virtual void Update() { }
        public virtual void FixedUpdate() 
        {
            if (
                ignoringPlatforms is true &&
                shouldIgnorePlatforms is false &&
                IsPlatformOverlapping() is false
            )
                SetPlatformIgnore(false);
        }
        public void ReadMovement(InputAction.CallbackContext context)
        {
            inputDirection = context.ReadValue<Vector2>();

            if (ignoringPlatforms is false && inputDirection.y < 0) SetPlatformIgnore(true);
            else if (ignoringPlatforms is true && inputDirection.y >= -0.1f) shouldIgnorePlatforms = false;
        }
        public abstract void Jump(InputAction.CallbackContext context);
        public abstract void Dash(InputAction.CallbackContext context);

        void SetPlatformIgnore(bool ignore)
        {
            foreach (var platformCollider in ActiveRoomGO.GetComponentsInChildren<Collider2D>())
            {
                if (platformCollider.gameObject.layer == platformLayerValue)
                {
                    Physics2D.IgnoreCollision(collider, platformCollider, ignore);
                }
            }
            ignoringPlatforms = ignore;
        }

        bool IsPlatformOverlapping()
        {
            Transform t = gameObject.transform;
            Vector2 size = new Vector2(collider.bounds.size.x, collider.bounds.size.y);
            return Physics2D.OverlapBox(t.position, size, t.rotation.eulerAngles.z, platformLayer);
        }
    }
}
