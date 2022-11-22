using UnityEngine;
using Game.Events;
using Game.Interfaces;
using System.Collections.Generic;

namespace Game.UI
{
    public class PlayerLifeBarManager : MonoBehaviour
    {
        Dictionary<LayeredBar, IHittable> _barContainerBinding = new Dictionary<LayeredBar, IHittable>();
        [SerializeField] LayeredBar[] _barContainers;

        private void Awake()
        {
            foreach (var container in _barContainers)
            {
                _barContainerBinding.Add(container, null);
                container.gameObject.SetActive(false);
            }
        }
        private void OnEnable()
        {
            EventManager.onPlayerSpawn += RegisterHittable;
            EventManager.onPlayerDespawn += UnregisterHittable;
        }
        private void OnDisable()
        {
            EventManager.onPlayerSpawn += RegisterHittable;
            EventManager.onPlayerDespawn -= UnregisterHittable;
        }
        private void Update()
        {
            foreach(var bar in _barContainerBinding.Keys)
            {
                var hittable = _barContainerBinding[bar];
                if (hittable != null) bar.UpdateFill(hittable.Health / hittable.MaxHealth);
            }
        }
        void RegisterHittable(GameObject hittableGameObject)
        {
            if(hittableGameObject.TryGetComponent(out IHittable hittable))
            {
                if (FirstUnassigned(out LayeredBar unassigned))
                {
                    _barContainerBinding[unassigned] = hittable;
                    unassigned.gameObject.SetActive(true);
                }
            }
        }
        void UnregisterHittable(GameObject gO)
        {
            if (gO.TryGetComponent(out IHittable gOHittable))
            {
                foreach(var container in _barContainerBinding.Keys)
                {
                    var assignedHittable = _barContainerBinding[container];
                    if(assignedHittable == gOHittable)
                    {
                        _barContainerBinding[container] = null;
                        container.gameObject.SetActive(false);
                        return;
                    }
                }
            }
        }
        bool FirstUnassigned(out LayeredBar unassigned)
        {
            unassigned = null;
            foreach(var container in _barContainerBinding.Keys)
            {
                if (_barContainerBinding[container] == null) 
                {
                    unassigned = container;
                    return true;
                }
            }
            return false;
        }
    }
}
