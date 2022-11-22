using Game.Utils;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IHittable
    {
        public float MaxHealth { get; }
        public float Health { get; set; }
        public Team Team { get; set; }
        public float Heal(float amount)
        {
            float max = MaxHealth - Health;
            amount = Mathf.Clamp(amount, 1, max);
            Health += amount; 
            return amount;
        }
        public float Hit(float damage) { Health -= damage; return damage; }
        public float Hit(float damage, Vector2 direction, float knockback);
    }
}
