using System.Collections.Generic;
using Newtonsoft.Json;
using Game.Events;

namespace Game.Unlocks
{
    [System.Serializable]
    public class UnlockStats
    {
        public UnlockStats()
        {
            EventManager.onPlayerLose += OnLose;
        }
        ~UnlockStats()
        {
            EventManager.onPlayerLose -= OnLose;
        }

        void OnWin() => wins++;
        void OnLose() => deaths++;

        public int deaths = 0;
        public int wins = 0;
    }
    [System.Serializable]
    public class UnlockData
    {
        public UnlockStats stats;
        public Dictionary<string, bool> unlocks;
        
        public UnlockData()
        {
            stats = new UnlockStats();
            unlocks = new Dictionary<string, bool>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
