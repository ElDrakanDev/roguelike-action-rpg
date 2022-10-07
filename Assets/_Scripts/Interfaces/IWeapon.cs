using UnityEngine;

namespace Game.Interfaces
{
    public interface IWeapon
    {
        public void Aim(Vector2 direction);
        public void UseBegin();
        public void Use();
        public void UseEnd();
        public void Drop(bool spawnPickable = true);
    }
}