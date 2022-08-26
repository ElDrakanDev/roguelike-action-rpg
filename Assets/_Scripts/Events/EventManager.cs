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
        public event Action onDoorEnter;
        public void OnDoorEnter() => onDoorEnter?.Invoke();
        public event Action onFinishGeneration;
        public void OnFinishGeneration() => onFinishGeneration?.Invoke();
        public event Action onNavigationExit;
        public void OnNavigationExit() => onNavigationExit?.Invoke();
        public static event Action<GameObject> onPlayerSpawn;
        public static void OnPlayerSpawn(GameObject player) => onPlayerSpawn?.Invoke(player);
        public static event Action<GameObject> onPlayerDespawn;
        public static void OnPlayerDespawn(GameObject player) => onPlayerDespawn?.Invoke(player);
        public event Action onRoomChange;
        public void OnRoomChange() => onRoomChange?.Invoke();
        #endregion
    }
}
