using UnityEngine;
using Game.Events;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Unlocks/BasicUnlock", order = 0)]
    public class UnlockScriptableObject : ScriptableObject
    {
        [SerializeField] string unlockName;
        [SerializeField] bool unlocked;
        public string Name { get => unlockName; }
        public bool Unlocked
        {
            get
            {
                UpdateUnlock();
                return unlocked;
            }
        }
        protected virtual void OnEnable()
        {
            EventManager.onUnlockLoad += UpdateUnlock;
        }

        protected virtual void OnDisable()
        {
            EventManager.onUnlockLoad -= UpdateUnlock;
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
    }
}
