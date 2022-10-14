using UnityEngine;

namespace Game.Stats
{
    public enum WeaponType { Melee, Ranged, Magic }

    [CreateAssetMenu(menuName = "Weapons/Stats")]
    public class WeaponStatsSO : ScriptableObject
    {
        public float damage = 5;
        public float useTime = 0.5f;
        public WeaponType type;
        public bool autoUse;
        public float projectileSpeed = 1;
        public float knockback = 1;

        public WeaponStats CreateStats()
        {
            return new WeaponStats(damage, useTime, type, autoUse, projectileSpeed, knockback);
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
        public float knockback = 1;

        public WeaponStats(
            float damage, float useTime, WeaponType type, bool autoUse, float projectileSpeed, float knockback
        )
        {
            this.damage = damage;
            this.useTime = useTime;
            this.type = type;
            this.autoUse = autoUse;
            this.projectileSpeed = projectileSpeed;
            this.knockback = knockback;
        }
    }
}
