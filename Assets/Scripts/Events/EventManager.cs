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
        public event Action onFinishGeneration;
        public void OnFinishGeneration() => onFinishGeneration?.Invoke();
        public event Action onRoomChange;
        public void OnRoomChange() => onRoomChange?.Invoke();
        #endregion
        }
    }
