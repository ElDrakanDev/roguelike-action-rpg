using UnityEngine;

namespace Game.Interfaces
{
    public interface IPickable : IInteractable
    {
        public void PickUp(GameObject other);
        public void Drop();
    }
}
