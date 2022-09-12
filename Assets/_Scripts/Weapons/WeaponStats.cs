using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName="Weapons/WeaponData")]
    public class WeaponStats : ScriptableObject
    {
        public float damage = 5;
        public float useTime = 0.5f;
        public WeaponType type;
        public bool autoUse;
        public float projectileSpeed = 1;
    }
}
