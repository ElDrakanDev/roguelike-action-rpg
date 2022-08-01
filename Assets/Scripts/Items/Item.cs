using UnityEngine;
using Game.Interfaces;
using Game.Players;

namespace Game.Items
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Item : MonoBehaviour, IPickable
    {
        protected GameObject owner;
        protected Player player;
        [SerializeField] string _title = "Item";
        [SerializeField] string _description = "Description";
        public Sprite sprite;
        public string Title { get => _title; }
        public string Description { get => _description; }

        private void OnEnable()
        {
            if (player)
            {
                AddEvents();
            }
        }
        private void OnDisable()
        {
            if (player)
            {
                RemoveEvents();
            }
        }
        public void PickUp(GameObject other)
        {
            if (!player)
            {
                owner = other;
                transform.parent = owner.transform;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                player = owner.GetComponent<Player>();
                owner.GetComponent<ItemContainer>().Add(this);
                AddEvents();
                OnPickUp();
            }
            else
                throw new System.Exception($"Tried picking up an already picked item.");
        }
        public void Drop()
        {
            if (player)
            {
                OnDrop();
                transform.parent = null;
                GetComponent<Collider2D>().enabled = true;
                GetComponent<SpriteRenderer>().enabled = true;
                owner.GetComponent<ItemContainer>().Remove(this);
                owner = null;
                player = null;
            }
            else
                throw new System.Exception($"Tried dropping an already dropped item.");
        }
        public virtual void OnPickUp(){ }
        public virtual void OnDrop() { }
        public void Interact(GameObject other)
        {
            PickUp(other);
        }
        public void Hover()
        {
            Debug.Log($"{Title} : {Description}");
        }
        protected virtual void AddEvents() { }
        protected virtual void RemoveEvents() { }
    }
}
