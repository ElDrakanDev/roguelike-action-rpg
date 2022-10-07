using Game.Events;
using Game.Interfaces;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(Weapon))]
    public class WeaponContainer : MonoBehaviour, IInteractable
    {
        [SerializeField] WeaponDataSO weaponData;
        Weapon weapon;
        public GameObject SelfPrefab { get => weaponData.pickable; }

        private void Awake()
        {
            weapon = GetComponent<Weapon>();
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
            Weapon weaponComponent = (Weapon)other.AddComponent(weapon.GetType());
            weaponComponent.Initialize(weaponData);
            //weapon.containerPrefab = SelfPrefab;
            //weapon.owner = other;
            //weapon.player = other.GetComponent<Player>();
            //weapon.player.weapon?.Drop();
            //weapon.sprite = GetComponent<SpriteRenderer>().sprite;
            //weapon.PickUp(other);
            Destroy(gameObject);
        }
        public void Inspect()
        {
            // Debug.Log($"{weaponData.Name} : {weaponData.Description}");
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
