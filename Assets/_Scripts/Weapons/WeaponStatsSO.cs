using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Stats")]
    public class WeaponStatsSO : ScriptableObject
    {
        public float damage = 5;
        public float useTime = 0.5f;
        public WeaponType type;
        public bool autoUse;
        public float projectileSpeed = 1;
        public AttackModes attackMode = AttackModes.Sequence;

        public WeaponStats CreateStats()
        {
            return new WeaponStats(damage, useTime, type, autoUse, projectileSpeed, attackMode);
        }
    }

    [System.Serializable]
    public class WeaponStats
    {
        public float damage;
        public float useTime = 0.5f;
        public WeaponType type;
        public bool autoUse;
        public float projectileSpeed = 1;
        public AttackModes attackMode;

        public WeaponStats(
            float damage, float useTime, WeaponType type, bool autoUse, float projectileSpeed, AttackModes attackMode
        )
        {
            this.damage = damage;
            this.useTime = useTime;
            this.type = type;
            this.autoUse = autoUse;
            this.projectileSpeed = projectileSpeed;
            this.attackMode = attackMode;
        }
    }
}
