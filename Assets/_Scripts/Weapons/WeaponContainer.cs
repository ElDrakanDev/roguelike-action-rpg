using UnityEngine;
using Game.Interfaces;
using Game.Players;

namespace Game.Weapons
{
    [RequireComponent(typeof(Collider2D))]
    public class WeaponContainer : MonoBehaviour, IInteractable
    {
        [SerializeField] WeaponData data;
        public void Interact(GameObject other)
        {
            data.owner = other;
            data.player = other.GetComponent<Player>();
            data.PickUp(other);
            Destroy(gameObject);
        }
        public void Hover()
        {
            Debug.Log($"{data.title} : {data.description}");
        }
    }
}