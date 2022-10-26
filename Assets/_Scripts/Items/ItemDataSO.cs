using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu()]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField] GameObject itemPrefab;
        [SerializeField] string itemName = "Example Item";
        [SerializeField] string itemDescription = "Does Stuff";
        [SerializeField] Sprite itemSprite;
        public Sprite ItemSprite { get { return itemSprite ? itemSprite : itemPrefab.GetComponent<SpriteRenderer>().sprite; } }
        public GameObject ItemPrefab { get { return itemPrefab; } }
        public string ItemName { get { return itemName; } }
        public string ItemDescription { get { return itemDescription; } }
    }
}
