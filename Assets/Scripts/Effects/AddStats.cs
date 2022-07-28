using System.Collections;
using System.Collections.Generic;
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
        public StatType type;
        public AttributeID attribute;
        public float amount;

        public override void Kill(EventData data)
        {
            if (data.destination.stat is Stat)
            {
                modifier = null;
                Stat stat = data.destination.stat as Stat;
                stat.Remove(this);
            }
        }
        public override void Run(EventData data)
        {
            if(data.destination.stat is Stat)
            {
                Debug.Log("Se creo la stat");
                Stat stat = data.destination.stat as Stat;
                modifier = new StatModifier(data.amount, this, stat, data.type);
                stat.Add(modifier);
                Debug.Log(stat.owner);
                Debug.Log($"Creada: valor {modifier.Value}, source {modifier.Source}, ");
            }
        }
    }
}
