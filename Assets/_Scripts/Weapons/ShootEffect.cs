using Game.Players;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "New shoot effect", menuName = "ScriptableObjects/Weapons/ShootEffect")]
    public class ShootEffect : WeaponEffect
    {
        [SerializeField] GameObject projectile;
        public override void Use(Player owner, Vector2 direction, float damage, WeaponType type, float speed)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            var instantiated = Instantiate(projectile, owner.transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
            instantiated.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
        public override void UseBegin(Player owner, Vector2 direction, float damage, WeaponType type, float speed)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            var instantiated = Instantiate(projectile, owner.transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
            instantiated.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
        public override void UseEnd(Player owner, Vector2 direction, float damage, WeaponType type, float speed)
        {

        }
    }
}
