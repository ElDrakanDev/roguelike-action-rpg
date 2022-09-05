using UnityEngine;
using Game.Players;

namespace Game.Weapons
{
    // [CreateAssetMenu(fileName = "New weapon effect", menuName = "ScriptableObjects/Weapons/WeaponEffect")]
    public abstract class WeaponEffect : ScriptableObject
    {
        public abstract void UseBegin(Player owner, Vector2 direction, float damage, WeaponType type, float speed);
        public abstract void Use(Player owner, Vector2 direction, float damage, WeaponType type, float speed);
        public abstract void UseEnd(Player owner, Vector2 direction, float damage, WeaponType type, float speed);
    }
}
