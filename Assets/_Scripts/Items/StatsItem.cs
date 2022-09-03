using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;

namespace Game.Items
{
    //public class StatsItem : Item
    //{
    //    [SerializeField] ItemStatValue[] stats;
    //    StatModifier[] _modifiers;

    //    public override void OnPickUp()
    //    {
    //        _modifiers = new StatModifier[stats.Length];
    //        for (int i = 0; i < stats.Length; i++)
    //        {
    //            _modifiers[i] = new StatModifier(stats[i].value, this, null, stats[i].type);
    //            player.stats.Add(_modifiers[i], stats[i].attribute);
    //        }
    //    }
    //    public override void OnDrop()
    //    {
    //        player.stats.Remove(this);
    //    }
    //}
}
