using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;
using Game.Players;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Stat Item Effect", menuName = "ItemEffects/StatItemEffect")]
    public class StatItemEffect : ItemEffect
    {

        public ItemStatModifier[] stats;
        public override void Apply(GameObject owner)
        {
            Player player = owner.GetComponent<Player>();
            
            foreach(var itemModifier in stats)
            {
                player.stats[itemModifier.id].Add(itemModifier.modifier);
            }
        }

        public override void Remove(GameObject owner)
        {
            Player player = owner.GetComponent<Player>();

            foreach (var itemModifier in stats)
            {
                player.stats[itemModifier.id].RemoveComponent(itemModifier.modifier);
            }
        }
    }

    [System.Serializable]
    public class ItemStatModifier
    {
        public AttributeID id;
        public StatModifier modifier;

        public ItemStatModifier(AttributeID id, StatModifier modifier)
        {
            this.id = id;
            this.modifier = modifier;
        }
    }
}
