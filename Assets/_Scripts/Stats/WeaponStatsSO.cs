using System.Collections.Generic;
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
        public List<AttributeID> flatAttributes = new List<AttributeID>();
        public List<AttributeID> multAttributes = new List<AttributeID>();

        public WeaponStats CreateStats()
        {
            return new WeaponStats(damage, useTime, type, autoUse, projectileSpeed, knockback, flatAttributes, multAttributes);
        }
    }

    [System.Serializable]
    public class WeaponStats
    {
        static Dictionary<WeaponType, List<AttributeID>> defaultMultAttributeScalers = new Dictionary<WeaponType, List<AttributeID>>(){
            {WeaponType.Melee, new List<AttributeID>() {AttributeID.Strength } },
            {WeaponType.Ranged, new List<AttributeID>() {AttributeID.Accuracy} },
            {WeaponType.Magic, new List<AttributeID>() {AttributeID.Intelligence} }
        };
        public float damage;
        public float useTime = 0.5f;
        public WeaponType type;
        public bool autoUse;
        public float projectileSpeed = 1;
        public float knockback = 1;
        public List<AttributeID> flatAttributes = new List<AttributeID>();
        public List<AttributeID> multAttributes = new List<AttributeID>();

        public WeaponStats(
            float damage,
            float useTime,
            WeaponType type,
            bool autoUse,
            float projectileSpeed,
            float knockback,
            List<AttributeID> flatScalers = null,
            List<AttributeID> multScalers = null
        )
        {
            this.damage = damage;
            this.useTime = useTime;
            this.type = type;
            this.autoUse = autoUse;
            this.projectileSpeed = projectileSpeed;
            this.knockback = knockback;
            flatAttributes = flatScalers is not null ? flatScalers : new List<AttributeID>();
            multAttributes = multScalers is not null ? multScalers : new List<AttributeID>();

            if (multAttributes.Count > 0) multAttributes = defaultMultAttributeScalers[type];
        }
    }
}
