using UnityEngine;

namespace Game.Interfaces
{
    public interface IWeapon : IPickable
    {
        public void Update();
        public void Aim(Vector2 direction);
        public void UseBegin();
        public void Use();
        public void UseEnd();
    }
}