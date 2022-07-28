using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Stats;
using Game.ID;

public class ShowStat : MonoBehaviour
{
    public Stat stat = new Stat(10, 1);
    [SerializeField] float statAmount = -1.111111111f;

    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.fKey.wasPressedThisFrame)
        {
            var modifier = new StatModifier(1, gameObject, stat, StatType.Flat);
            stat.Add(modifier);
            statAmount = stat.Value;
        }
        else if (keyboard.mKey.wasPressedThisFrame)
        {
            var modifier = new StatModifier(0.15f, gameObject, stat, StatType.Mult);
            stat.Add(modifier);
            statAmount = stat.Value;
        }
    }
}
