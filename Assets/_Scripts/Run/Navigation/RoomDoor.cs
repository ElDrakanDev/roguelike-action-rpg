using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using System.Threading.Tasks;
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
            EventManager.OnDoorEnter();
        }
    }
}
