using UnityEngine;
using Game.States;
using Game.Stats;
using UnityEngine.InputSystem;
using Game.Interfaces;

namespace Game.Players
{
    public class PlayerController : Context<ControllerState>
    {
        readonly GameObject gameObject;
        readonly Player player;
        readonly BoxCollider2D collider;
        //Vector2 direction;
        LayerMask interactableLayer;
        public Vector2 point, size;

        public new ControllerState Current 
        { 
            get 
            {
                if(_current is null)
                    _current = new GroundedMoveState(gameObject, player);
                return _current;
            }
            protected set => _current = value;
        }

        public PlayerController(ControllerState state, Player player, GameObject gameObject) : base(state) 
        {
            this.gameObject = gameObject;
            this.player = player;
            interactableLayer = LayerMask.GetMask("Interactable");
            collider = gameObject.GetComponent<BoxCollider2D>();
            if (state is null) Current = new GroundedMoveState(gameObject, player);
        }
        public void Jump(InputAction.CallbackContext context) => Current.Jump(context);
        public void Move(InputAction.CallbackContext context) => Current.ReadMovement(context);
        public void Update() => Current.Update();
        public void FixedUpdate() => Current.FixedUpdate();

        public void Dash(InputAction.CallbackContext context) => Current.Dash(context);
        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.started) return;

            Collider2D[] interactableColliders = Physics2D.OverlapBoxAll(collider.transform.position, collider.bounds.size, collider.transform.rotation.z, interactableLayer);
            
            if (interactableColliders != null && interactableColliders.Length > 0)
            {
                float closestDist = float.MaxValue;
                GameObject closest = null;

                foreach (var col in interactableColliders)
                {
                    var interactableGameObject = col.gameObject;
                    float distance = Vector2.Distance(interactableGameObject.transform.position, gameObject.transform.position);

                    if(distance <= closestDist)
                    {
                        closest = interactableGameObject;
                    }
                }

                foreach(var interactable in closest.GetComponents<IInteractable>())
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}
