using UnityEngine;
using Game.Events;
using UnityEngine.InputSystem;
using Game.Interfaces;
using Game.Input;
using Game.Utils;

namespace Game.Players
{
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D rb;
        Player player;
        BoxCollider2D boxCollider;
        Vector2 direction;
        LayerMask interactableLayer;
        [HideInInspector] public Vector2 point, size;
        bool usingWeapon = false;
        GameObject closestInteractable;
        ControllerState _current;
        PlayerActionsControls playerControls;
        SpriteAnimator animator;
        SpriteRenderer spriteRenderer;
        bool facingRightOnStart;
        bool FacingRight { get => facingRightOnStart && !spriteRenderer.flipX; }

        [Header("Animation")]
        [SerializeField] SpriteAnimationSO idleAnimation;
        [SerializeField] SpriteAnimationSO runAnimation;
        [SerializeField] SpriteAnimationSO airUpAnimation;
        [SerializeField] SpriteAnimationSO airDownAnimation;
        [SerializeField] float runAnimationSpeedMultiplier = 0.4f;
        [SerializeField] float minRunAnimationScale = 0.5f;
        [SerializeField] float maxRunAnimationScale = 3;

        public ControllerState Current
        {
            get
            {
                if (_current is null)
                    _current = new GroundedMoveState(gameObject, player);
                return _current;
            }
            protected set => _current = value;
        }
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<Player>();
            animator = GetComponent<SpriteAnimator>();
            playerControls = new PlayerActionsControls();
            interactableLayer = LayerMask.GetMask("Interactable");
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
            Current = new GroundedMoveState(gameObject, player);
            spriteRenderer = GetComponent<SpriteRenderer>();
            facingRightOnStart = spriteRenderer.flipX is false;
        }
        private void OnEnable() => playerControls?.Enable();
        private void OnDisable() => playerControls?.Disable();
        void Update()
        {
            Current.Update();
            if (usingWeapon) player.weapon?.Use();
            player.weapon?.Aim(direction);

            UpdateAnimation();
        }
        void FixedUpdate()
        {
            Current.FixedUpdate();
            CheckInteractables();
        }
        #region Animation
        void UpdateAnimation()
        {
            Vector3 velocity = rb.velocity;
            float timeScale;
            if (velocity.y > 0.05f)
            {
                animator.SetAnimation(airUpAnimation, 1);
            }
            else if (velocity.y < -0.05f)
            {
                animator.SetAnimation(airDownAnimation, 1);
            }
            else if (direction.x > 0.05f)
            {
                timeScale = Mathf.Clamp(velocity.x * runAnimationSpeedMultiplier, minRunAnimationScale, maxRunAnimationScale);
                animator.SetAnimation(runAnimation, timeScale);
            }
            else if (direction.x < -0.05f)
            {
                timeScale = Mathf.Clamp(-velocity.x * runAnimationSpeedMultiplier, minRunAnimationScale, maxRunAnimationScale);
                animator.SetAnimation(runAnimation, timeScale);
            }
            else
            {
                animator.SetAnimation(idleAnimation, 1);
            }
            FaceDirection();
        }
        void FaceDirection()
        {
            if(
                (direction.x > 0 && FacingRight is false) ||
                (direction.x < 0 && FacingRight is true)
            )
                spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        #endregion
        #region Controls
        public void Move(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>();
            Current.ReadMovement(context);
        }
        public void Jump(InputAction.CallbackContext context) => Current.Jump(context);
        public void Dash(InputAction.CallbackContext context) => Current.Dash(context);
        public void MainAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                player.weapon?.UseBegin();
                usingWeapon = true;
            }
            else if (context.canceled)
            {
                usingWeapon = false;
                player.weapon?.UseEnd();
            }
        }
        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.started || closestInteractable is null) return;

            foreach (var interactable in closestInteractable.GetComponents<IInteractable>())
            {
                interactable.Interact(gameObject);
            }
        }
        #endregion

        void CheckInteractables()
        {
            Collider2D[] interactableColliders = Physics2D.OverlapBoxAll(boxCollider.transform.position, boxCollider.bounds.size, boxCollider.transform.rotation.z, interactableLayer);
            if (interactableColliders != null && interactableColliders.Length > 0)
            {
                float closestDist = float.MaxValue;
                GameObject closest = null;

                foreach (var col in interactableColliders)
                {
                    var interactableGameObject = col.gameObject;
                    float distance = Vector2.Distance(interactableGameObject.transform.position, gameObject.transform.position);

                    if (distance <= closestDist)
                    {
                        closest = interactableGameObject;
                    }
                }

                closestInteractable = closest;
            }
            else closestInteractable = null;
            EventManager.OnInteractableInspect(gameObject, closestInteractable);
        }
    }
}
