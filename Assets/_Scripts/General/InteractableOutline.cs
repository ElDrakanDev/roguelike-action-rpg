using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Events;

namespace Game.General
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractableOutline : MonoBehaviour, IInteractable
    {
        [SerializeField] ToggleOutline outline;
        SpriteRenderer spriteRenderer;
        GameObject currentInspector = null;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable() => EventManager.onInteractableInspect += CheckInspect;
        private void OnDisable() => EventManager.onInteractableInspect -= CheckInspect;
        public void Inspect()
        {
            spriteRenderer.material = outline.hoverMaterial;
        }
        public void Interact(GameObject interactor) { }
        void CheckInspect(GameObject inspector, GameObject inspected)
        {
            if(inspected == gameObject)
            {
                Inspect();
                currentInspector = inspector;
            }
            else if(currentInspector == inspector || currentInspector is null)
            {
                spriteRenderer.material = outline.normalMaterial;
                currentInspector = null;
            }
        }
    }
}
