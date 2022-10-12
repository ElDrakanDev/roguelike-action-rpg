using UnityEngine;

namespace Game.Interfaces
{
    public interface IHittable
    {
        public float Hit(float damage);
        public float Hit(float damage, Vector2 direction, float knockback);
    }
}
