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
            EventManager.onPlayerLose -= UpdateUnlock;
        }
        protected override void OnEnable()
        {
            EventManager.onPlayerLose += UpdateUnlock;
        }

        void UpdateUnlock()
        {
            if(UnlockManager.Instance.saveFileData.stats.deaths >= minLosses) SetUnlocked(true);
        }
    }
}
