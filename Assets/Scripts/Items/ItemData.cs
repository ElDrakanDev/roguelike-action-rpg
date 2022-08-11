using UnityEngine;
using Game.Players;
using Game.Interfaces;

namespace Game.Items
{
    [System.Serializable]
    public class ItemData : IPickable
    {
        public ItemEffect[] effects;
        public string title = "Item";
        public string description = "Description";
        [HideInInspector] public Sprite sprite;
        [HideInInspector] public Player player;
        [HideInInspector] public GameObject owner;
        public ItemData(ItemEffect[] effects, string title, string description, Sprite sprite = null, GameObject owner = null)
        {
            this.effects = effects;
            this.owner = owner;
            this.title = title;
            this.description = description;
            this.sprite = sprite;

            if (owner) PickUp(owner);
        }

        public void PickUp(GameObject origin)
        {
            Drop();

            owner = origin;

            foreach(var effect in effects)
            {
                effect.Apply(owner);
            }
        }

        public void Drop()
        {
            if (owner)
            {
                foreach (var effect in effects)
                {
                    effect.Remove(owner);
                }
            }
        }
    }
}
