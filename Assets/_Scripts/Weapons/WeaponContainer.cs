using Game.Events;
using Game.Interfaces;
using Game.Players;
using Game.Utils;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponContainer : MonoBehaviour, IInteractable
    {
        [SerializeField] public Weapon weapon;
        [SerializeReference] PrefabReference selfReference;
        public GameObject SelfPrefab { get => selfReference.prefab; }

        void Start()
        {
            if (TryGetComponent(out BoxCollider2D collider) is false) collider = gameObject.AddComponent<BoxCollider2D>();
            if (TryGetComponent(out Rigidbody2D rb) is false) rb = gameObject.AddComponent<Rigidbody2D>();
        }
        void OnEnable()
        {
            EventManager.onInteractableInspect += CheckInspect;
        }
        private void OnDisable()
        {
            EventManager.onInteractableInspect -= CheckInspect;   
        }
        public void Interact(GameObject other)
        {
            weapon.containerPrefab = SelfPrefab;
            weapon.owner = other;
            weapon.player = other.GetComponent<Player>();
            weapon.player.weapon?.Drop();
            weapon.sprite = GetComponent<SpriteRenderer>().sprite;
            weapon.PickUp(other);
            Destroy(gameObject);
        }
        public void Inspect()
        {
            Debug.Log($"{weapon.title} : {weapon.description}");
        }

        void CheckInspect(GameObject inspector, GameObject hovered)
        {
            if(hovered == gameObject)
            {
                Inspect();
            }
        }
    }
}
