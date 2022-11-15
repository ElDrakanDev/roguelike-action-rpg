using UnityEngine;
using Game.Events;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Unlocks/BasicUnlock", order = 0)]
    public class UnlockScriptableObject : ScriptableObject
    {
        protected bool alwaysHidden = false;
        public bool Hidden { get => alwaysHidden; }
        [SerializeField] bool unlocked;
        [field:SerializeField] public string Name { get; protected set; }
        [field:SerializeField] public string Description { get; protected set; }
        [field:SerializeField] public Sprite Icon { get; protected set; }
        public virtual bool IsUnlocked()
        {
            UpdateUnlock();
            return unlocked;
        }
        protected virtual void OnEnable()
        {
            EventManager.onUnlockLoad += UpdateUnlock;
            EventManager.onUnlock += WasNewSelfUnlock;
        }

        protected virtual void OnDisable()
        {
            EventManager.onUnlockLoad -= UpdateUnlock;
            EventManager.onUnlock -= WasNewSelfUnlock;
        }

        public void SetUnlocked(bool unlocked = true)
        {
            this.unlocked = unlocked;
            UnlockManager.Instance.SetUnlock(Name, unlocked);
        }

        void UpdateUnlock()
        {
            unlocked = UnlockManager.Instance.IsUnlocked(Name);
        }

        void WasNewSelfUnlock(string unlockedName)
        {
            if (Name == unlockedName) EventManager.OnUnlockShow(Name, Description, Icon);
        }
    }
}
