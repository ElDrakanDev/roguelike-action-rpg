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
        public abstract void Update();
        public abstract void FixedUpdate();
        public void ReadMovement(InputAction.CallbackContext context)
        {
            inputDirection = context.ReadValue<Vector2>();

            if (!ignoringPlatforms && inputDirection.y < 0) SetPlatformIgnore(true);
            else if (ignoringPlatforms && inputDirection.y >= 0) SetPlatformIgnore(false);
        }
        public abstract void Jump(InputAction.CallbackContext context);
        public abstract void Dash(InputAction.CallbackContext context);

        void SetPlatformIgnore(bool ignore)
        {
            Debug.Log($"IgnoreLayer: {platformLayerValue}");
            foreach (var platformCollider in ActiveRoomGO.GetComponentsInChildren<Collider2D>())
            {
                Debug.Log($"{platformCollider} layer: {platformCollider.gameObject.layer}. Affected = {platformCollider.gameObject.layer == platformLayerValue}. Ignore = {ignore}");
                if (platformCollider.gameObject.layer == platformLayerValue)
                {
                    Physics2D.IgnoreCollision(collider, platformCollider, ignore);
                }
            }
            ignoringPlatforms = ignore;
        }
    }
}
