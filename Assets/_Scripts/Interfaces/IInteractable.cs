using UnityEngine;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        public void Interact(GameObject interactor);
        public void Hover();
    }
}

