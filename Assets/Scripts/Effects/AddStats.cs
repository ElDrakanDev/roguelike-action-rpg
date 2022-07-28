using UnityEngine;
using Game.Stats;
using Game.ID;
using Game.Events;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/AddStat")]
    public class AddStats : Effect
    {
        StatModifier modifier;
        Stat stat;
        public StatType type;
        public AttributeID attribute;
        public float amount;

        public override void Kill(EventData data= null)
        {
            if (stat != null)
            {
                modifier = null;
                stat.Remove(this);
                stat = null;
            }
        }
        public override void Run(EventData data= null)
        {
            if(data.destination.stat is Stat)
            {
                stat = data.destination.stat as Stat;
                modifier = new StatModifier(data.amount, this, stat, data.type);
                stat.Add(modifier);
                Debug.Log(stat.owner);
            }
        }
    }
}
