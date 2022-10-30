using UnityEngine;
using Game.Interfaces;
using Game.Events;

namespace Game.Run
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomDoor : MonoBehaviour, IInteractable
    {
        const float PIXEL_SIZE = 1f / 32f;
        void Start()
        {
            if(TryGetComponent(out SpriteRenderer renderer))
            {
                var layerMask = LayerMask.GetMask("Ground", "Platform");
                var hit = Physics2D.Raycast(transform.position, Vector2.down, renderer.bounds.size.y, layerMask);
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - hit.distance + renderer.bounds.size.y * 0.5f - (float)PIXEL_SIZE * transform.lossyScale.y,
                    0
                );
            }
        }
        public void Inspect()
        {
            
        }

        public void Interact(GameObject interactor)
        {
            if(Run.instance.navigator.CanMove) EventManager.OnDoorEnter();
        }
    }
}
