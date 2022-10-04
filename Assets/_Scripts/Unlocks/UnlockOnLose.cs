using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Unlocks/OnLose")]
    public class UnlockOnLose : UnlockScriptableObject
    {
        [SerializeField] int minLosses;

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.onPlayerLose -= AutoUnlock;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.onPlayerLose += AutoUnlock;
        }

        void AutoUnlock()
        {
            if(UnlockManager.Instance.saveFileData.stats.deaths >= minLosses) SetUnlocked(true);
        }
    }
}
