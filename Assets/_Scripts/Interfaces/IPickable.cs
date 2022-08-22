using UnityEngine;

namespace Game.Interfaces
{
    public interface IPickable
    {
        public void PickUp(GameObject other);
        public void Drop();
    }
}
