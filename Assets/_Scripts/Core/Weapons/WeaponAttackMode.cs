using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public enum AttackMode
    {
        Sequence, Random
    }
    public abstract class WeaponAttackMode
    {
        public void Update() { }
        public abstract WeaponAttack[] ChooseAttacks(WeaponAttack[] attacks, bool canceled = false);

        public static WeaponAttackMode FromEnum(AttackMode mode)
        {
            Dictionary<AttackMode, WeaponAttackMode> modes = new Dictionary<AttackMode, WeaponAttackMode>()
            {
                {AttackMode.Sequence, new SequenceAttackMode()},
                {AttackMode.Random, new RandomAttackMode()}
            };
            return modes[mode];
        }
    }

    public class SequenceAttackMode : WeaponAttackMode
    {
        byte calls = 0;
        public override WeaponAttack[] ChooseAttacks(WeaponAttack[] attacks, bool canceled = false)
        {
            if(!canceled) calls++;
            return new WeaponAttack[1] { attacks[calls % attacks.Length] };
        }
    }

    public class RandomAttackMode : WeaponAttackMode
    {
        public override WeaponAttack[] ChooseAttacks(WeaponAttack[] attacks, bool canceled = false)
        {
            return new WeaponAttack[1] { attacks[Random.Range(0, attacks.Length)] };
        }
    }
}
