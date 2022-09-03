using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class ItemContainer : MonoBehaviour
    {
        [SerializeField] List<ItemData> items = new List<ItemData>();

        public void Add(ItemData item)
        {
            items.Add(item);
        }

        public void Remove(ItemData item)
        {
            items.Remove(item);
        }
    }
}
