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
        public string Title { get => _title; }
        public string Description { get => _description; }
        protected bool pickedUp = false;
        public void PickUp(GameObject other)
        {
            if (!pickedUp)
            {
                owner = other;
                player = other.GetComponent<Player>();
                pickedUp = true;
                AddEvents();
                OnPickUp();
            }
            else
                throw new System.Exception($"Tried picking up an already picked item.");
        }
        public void Drop()
        {
            if (pickedUp)
            {
                owner = null;
                player = null;
                pickedUp = false;
                OnDrop();
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

        
        /*//Player owner;
ItemData data;
public void PickUp(GameObject other)
{
   try
   {
       for (int i = 0; i < data.events.Length; i++)
       {
           data.effects[i].Init(new EventData(data.amounts[i], null, other.GetComponent("ShowStat"), data.types[i]));
           data.events[i].AddListener(data.effects[i].Run);
       }
   }
   catch(IndexOutOfRangeException ex)
   {
       Debug.LogWarning($"No coincidieron los indices de eventos de item y efectos. {ex}");
   }
}

public void Drop()
{
   try
   {
       for (int i = 0; i < data.events.Length; i++)
       {
           data.events[i].RemoveListener(data.effects[i].Run);
           data.effects[i].Kill();
       }
   }
   catch (IndexOutOfRangeException ex)
   {
       Debug.LogWarning($"No coincidieron los indices de eventos de item y efectos. {ex}");
   }
   owner = null;
}

public void Interact(GameObject other)
{
   owner = other;
   PickUp(other);
}
*/
    }
}
