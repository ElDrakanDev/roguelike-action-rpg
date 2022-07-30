using UnityEngine;
using Game.Stats;
using Game.Events;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/AddStat")]
    public class AddStats : Effect
    {
        StatModifier modifier;
        Stat stat;
        [HideInInspector] public StatType type;
        [HideInInspector] public AttributeID attribute;
        [HideInInspector] public float amount;

        public override void Init(EventData data = null)
        {
            if(data.destination.stat is Stat)
            {
                stat = data.destination.stat as Stat;
                modifier = new StatModifier(data.amount, this, stat, data.type);
                stat.Add(modifier);
            }
        }

        public override void Kill(EventData data= null)
        {
            if (stat != null)
            {
                modifier = null;
                stat.Remove(this);
                stat = null;
            }
        }
    }
}
