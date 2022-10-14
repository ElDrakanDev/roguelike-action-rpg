using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Stats;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Data")]
    public class WeaponDataSO : ScriptableObject
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public Sprite Sprite { get; protected set; }
        public GameObject pickable;
        public WeaponStatsSO statsScriptable;
        public WeaponAttack[] attacks;
        public AttackMode attackMode = AttackMode.Sequence;
    }
}
