using UnityEngine;
using Game.Interfaces;
using Game.Players;
using Game.Generation;
using System.Collections.Generic;

namespace Game.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class Item : MonoBehaviour, IPickable
    {
        [HideInInspector] public GameObject owner;
        [HideInInspector] public Player player;
        public ItemDataSO data;
        public Dictionary<string, float> persistentData = new Dictionary<string, float>();

        public void PickUp(GameObject other)
        {
            owner = other;
            player = owner.GetComponent<Player>();
            ItemEffect();
        }
        public void Drop()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 361)));
            Instantiate(data.ItemPrefab, position, rotation, Room.ActiveRoom.transform);
            Destroy(this);
        }
        private void Update()
        {
            if (player is not null) ItemUpdate(); 
        }
        private void FixedUpdate()
        {
            if (player is not null) ItemFixedUpdate();
        }
        protected virtual void ItemUpdate() { }
        protected virtual void ItemFixedUpdate() { }
        protected virtual void ItemEffect() { }
        protected virtual void CancelEffect() { }
    }
}
