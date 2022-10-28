using UnityEngine;
using Game.Players;
using Game.Stats;

namespace Game.Weapons
{
    // [CreateAssetMenu(fileName = "New weapon effect", menuName = "ScriptableObjects/Weapons/WeaponEffect")]
    public abstract class WeaponAttack : ScriptableObject
    {
        public abstract void UseBegin(ref WeaponAttackInfo info);
        public abstract void Use(ref WeaponAttackInfo info);
        public abstract void UseEnd(ref WeaponAttackInfo info);
    }
    public struct WeaponAttackInfo
    {
        public Player owner;
        public Vector2 direction;
        public float damage;
        public float knockback;
        public float speed;
        public float useTime;

        public WeaponAttackInfo(Player owner, Vector2 direction, float damage, float knockback, float speed, float useTime)
        {
            this.owner = owner;
            this.direction = direction;
            this.damage = damage;
            this.knockback = knockback;
            this.speed = speed;
            this.useTime = useTime;
        }
    }
}
