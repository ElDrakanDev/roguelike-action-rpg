using UnityEngine;
using Game.Interfaces;
using UnityEngine.InputSystem;
using Game.Stats;

namespace Game.Players
{
    public abstract class ControllerState : MonoBehaviour, IState
    {
        float _timescaledMaxSpeed => 10f * _timeScale;

        protected Rigidbody2D rb;
        protected BoxCollider2D boxCollider;
        protected Player player;
        protected LayerMask groundLayer;
        protected LayerMask platformLayer;
        protected Vector2 inputDirection;
        bool ignoringPlatforms = false;
        bool shouldIgnorePlatforms;
        int platformLayerValue;
        protected float _timeScale { get => player.TimeScale; set => player.TimeScale = value; }
        protected Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }
        protected Vector3 Position { get => gameObject.transform.position; set => gameObject.transform.position = value; }
        GameObject ActiveRoomGO { get => Run.Run.instance.ActiveRoom.gameObject; }
        public float Speed
        {
            get => Mathf.Clamp((player.stats[AttributeID.Agility].Value + 1) * 0.05f, 0.05f, 0.2f) * _timeScale;
        }
        protected float _maxSpeed { get => Mathf.Clamp(_timescaledMaxSpeed + (player.stats[AttributeID.Agility].Value - 1), _timescaledMaxSpeed - 3, _timescaledMaxSpeed + 5); }
        public IContext<IState> Context { get; set; }

        void Awake()
        {
            player = GetComponent<Player>();
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            groundLayer = LayerMask.GetMask("Ground", "Platform");
            platformLayer = LayerMask.GetMask("Platform");
            platformLayerValue = LayerMask.NameToLayer("Platform");
        }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() 
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
                    Physics2D.IgnoreCollision(boxCollider, platformCollider, ignore);
                }
            }
            ignoringPlatforms = ignore;
        }

        bool IsPlatformOverlapping()
        {
            Transform t = gameObject.transform;
            Vector2 size = new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y);
            return Physics2D.OverlapBox(t.position, size, t.rotation.eulerAngles.z, platformLayer);
        }
    }
}
