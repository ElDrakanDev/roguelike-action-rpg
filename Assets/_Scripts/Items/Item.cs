using UnityEngine;
using Game.Interfaces;
using Game.Players;

namespace Game.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class Item : MonoBehaviour, IInteractable
    {
        [SerializeField] ItemData data;

        public void Interact(GameObject other)
        {
            data.owner = other;
            data.player = other.GetComponent<Player>();
            other.GetComponent<ItemContainer>().Add(data);
            data.PickUp(other);
            Destroy(gameObject);
        }
        public void Hover()
        {
            Debug.Log($"{data.title} : {data.description}");
        }
    }
}
