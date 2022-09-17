using UnityEngine;
using Game.Players;

namespace Game.Weapons
{
    // [CreateAssetMenu(fileName = "New weapon effect", menuName = "ScriptableObjects/Weapons/WeaponEffect")]
    public abstract class WeaponAttack : ScriptableObject
    {
        public abstract void UseBegin(Player owner, Vector2 direction, WeaponStats stats);
        public abstract void Use(Player owner, Vector2 direction, WeaponStats stats);
        public abstract void UseEnd(Player owner, Vector2 direction, WeaponStats stats);
    }
}
