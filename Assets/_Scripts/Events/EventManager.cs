using UnityEngine;
using System;

namespace Game.Events
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance;

        private void Awake()
        {
            if (!instance)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region Events
        public static event Action onDoorEnter;
        public static void OnDoorEnter() => onDoorEnter?.Invoke();
        public static event Action onFinishGeneration;
        public static void OnFinishGeneration() => onFinishGeneration?.Invoke();
        public static event Action<GameObject, GameObject> onInteractableInspect;
        public static void OnInteractableInspect(GameObject inspector, GameObject inspected) => onInteractableInspect?.Invoke(inspector, inspected);
        public static event Action onNavigationExit;
        public static void OnNavigationExit() => onNavigationExit?.Invoke();
        public static event Action<GameObject> onPlayerSpawn;
        public static void OnPlayerSpawn(GameObject player) => onPlayerSpawn?.Invoke(player);
        public static event Action<GameObject> onPlayerDespawn;
        public static void OnPlayerDespawn(GameObject player) => onPlayerDespawn?.Invoke(player);
        public static event Action onRoomChange;
        public static void OnRoomChange() => onRoomChange?.Invoke();
        #endregion
    }
}
