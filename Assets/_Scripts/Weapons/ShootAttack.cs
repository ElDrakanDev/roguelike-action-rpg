using Game.Players;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "New shoot effect", menuName = "ScriptableObjects/Weapons/ShootEffect")]
    public class ShootAttack : WeaponAttack
    {
        struct ShootData
        {
            public GameObject gun;
            public readonly float gunWidth;
            public readonly SpriteRenderer gunRenderer;

            public ShootData(GameObject gun)
            {
                this.gun = gun;
                this.gunRenderer = gun.GetComponent<SpriteRenderer>();
                this.gunWidth = gunRenderer.bounds.size.x;
            }
        }

        [SerializeField] GameObject projectile;
        [SerializeField] GameObject gun;
        [SerializeField] float gunMargin = 0.5f;
        Dictionary<Player, ShootData> shotsDict = new Dictionary<Player, ShootData>();
        public override void Use(Player owner, Vector2 direction, WeaponStats stats)
        {
            Shoot(owner, direction, stats);
        }
        public override void UseBegin(Player owner, Vector2 direction, WeaponStats stats)
        {
            Shoot(owner, direction, stats);
        }
        public override void UseEnd(Player owner, Vector2 direction, WeaponStats stats)
        {
            RemoveGun(owner);
        }
        void Shoot(Player owner, Vector2 direction, WeaponStats stats)
        {
            float zRotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);

            if (shotsDict.TryGetValue(owner, out ShootData data))
            {
                ShootWithData(data, owner, direction, zRotation, stats.projectileSpeed);
                return;
            }
            var newGun = Instantiate(gun);
            newGun.transform.SetParent(owner.transform);
            data = new ShootData(newGun);
            shotsDict.Add(owner, data);
            ShootWithData(data, owner, direction, zRotation, stats.projectileSpeed);
        }
        void ShootWithData(ShootData data, Player owner, Vector2 direction, float zRotation, float speed)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, zRotation);
            data.gun.transform.position = owner.transform.position + new Vector3(direction.x * gunMargin, direction.y * gunMargin, 0);
            data.gun.transform.rotation = rotation;
            data.gunRenderer.flipY = zRotation > 90 || zRotation < -90 ? true : false;
            Vector3 shootPos = data.gun.transform.position + new Vector3(direction.x * data.gunWidth * 0.8f, direction.y * data.gunWidth * 0.8f, 0);
            var newProjectile = Instantiate(projectile, shootPos, rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * speed;
            return;
        }
        void RemoveGun(Player owner)
        {
            if(shotsDict.TryGetValue(owner, out ShootData data))
            {
                Destroy(data.gun);
                shotsDict.Remove(owner);
            }
        }
    }
}
