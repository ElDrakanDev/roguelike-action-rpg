using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class ItemContainer : MonoBehaviour, IInteractable
    {
        public ItemDataSO data;
        public void Interact(GameObject other)
        {
            var item = GetComponent<Item>();
            item = (Item)other.AddComponent(item.GetType());
            item.data = data;
            item.PickUp(other);
            Destroy(gameObject);
        }
        public void Inspect()
        {
            Debug.Log($"{data.ItemName} : {data.ItemDescription}");
        }
    }
}
