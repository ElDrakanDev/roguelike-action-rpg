using UnityEngine;
using Game.Events;
using System.Threading.Tasks;

namespace Game.Unlocks
{
    [CreateAssetMenu(menuName = "Unlocks/OnLose")]
    public class UnlockOnLose : UnlockScriptableObject
    {
        [SerializeField] int minLosses;
        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.onPlayerLose += AutoUnlock;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.onPlayerLose -= AutoUnlock;
        }
        async void AutoUnlock()
        {
            await Task.Delay(1);
            if(UnlockManager.Instance.saveFileData.stats.deaths >= minLosses) SetUnlocked(true);  
        }
    }
}
