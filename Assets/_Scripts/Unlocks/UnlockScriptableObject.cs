using UnityEngine;
using Game.Events;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Unlocks/BasicUnlock", order = 0)]
    public class UnlockScriptableObject : ScriptableObject
    {
        protected bool alwaysHidden = false;
        public bool Hidden { get => alwaysHidden; }
        [SerializeField] string unlockName;
        [SerializeField] bool unlocked;
        public string Name { get => unlockName; }
        public virtual bool IsUnlocked()
        {
            UpdateUnlock();
            return unlocked;
        }
        protected virtual void OnEnable()
        {
            EventManager.onUnlockLoad += UpdateUnlock;
            EventManager.onUnlock += ShowUnlock;
        }

        protected virtual void OnDisable()
        {
            EventManager.onUnlockLoad -= UpdateUnlock;
            EventManager.onUnlock -= ShowUnlock;
        }

        public void SetUnlocked(bool unlocked = true)
        {
            this.unlocked = unlocked;
            UnlockManager.Instance.SetUnlock(unlockName, unlocked);
        }

        void UpdateUnlock()
        {
            unlocked = UnlockManager.Instance.IsUnlocked(unlockName);
        }

        void ShowUnlock(string unlockedName)
        {
            if(unlockName == unlockedName)
            {
                Debug.Log($"{unlockName} desbloqueado.");
            }
        }
    }
}
