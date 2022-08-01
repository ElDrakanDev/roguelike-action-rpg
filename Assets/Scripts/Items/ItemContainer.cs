using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class ItemContainer : MonoBehaviour
    {
        List<Item> items = new List<Item>();

        public void Add(Item item)
        {
            items.Add(item);
        }

        public void Remove(Item item)
        {
            items.Remove(item);
        }
    }
}
