using UnityEngine;
using Game.Interfaces;
using Game.Input;
using Game.Events;

namespace Game.Run
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomDoor : MonoBehaviour, IInteractable
    {
        [SerializeField] PlayerActionsControls controls;
        private void Awake()
        {
            controls = new PlayerActionsControls();
        }
        public void Inspect()
        {
            throw new System.NotImplementedException();
        }

        public void Interact(GameObject interactor)
        {
            if(Run.instance.navigator.CanMove) EventManager.OnDoorEnter();
        }
    }
}
