using Game.Players;
using UnityEngine;
using System.Collections.Generic;
using Game.Projectiles;
using Game.Utils;
using Game.Stats;

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
            public readonly Vector3 gunScale;

            public ShootData(GameObject gun)
            {
                this.gun = gun;
                this.gunRenderer = gun.GetComponent<SpriteRenderer>();
                this.gunWidth = gunRenderer.bounds.size.x;
                gunScale = gun.transform.localScale;

            }
        }

        [SerializeField] ProjectileDataSO projData;
        [SerializeField] GameObject gun;
        [SerializeField] float gunMargin = 0.5f;
        Dictionary<Player, ShootData> shotsDict = new Dictionary<Player, ShootData>();
        public override void Use(ref WeaponAttackInfo info)
        {
            Shoot(info);
        }
        public override void UseBegin(ref WeaponAttackInfo info)
        {
            Shoot(info);
        }
        public override void UseEnd(ref WeaponAttackInfo info)
        {
            RemoveGun(info.owner);
        }
        void Shoot(WeaponAttackInfo info)
        {
            Vector2 direction = info.direction;
            Player owner = info.owner;
            if (direction == Vector2.zero) return;

            if (shotsDict.TryGetValue(owner, out ShootData data))
            {
                ShootWithData(data, owner, direction, info);
                return;
            }
            var newGun = Instantiate(gun);
            newGun.transform.SetParent(owner.transform);
            data = new ShootData(newGun);
            shotsDict.Add(owner, data);
            ShootWithData(data, owner, direction, info);
        }
        void ShootWithData(ShootData data, Player owner, Vector2 direction, WeaponAttackInfo info)
        {
            float zRotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            Quaternion rotation = Quaternion.Euler(0, 0, zRotation);
            bool gunFacingLeft = rotation.eulerAngles.z > 90 && rotation.eulerAngles.z < 270;

            data.gun.transform.SetPositionAndRotation(owner.transform.position + new Vector3(direction.x * gunMargin, direction.y * gunMargin, 0), rotation);
            Vector3 newScale = data.gunScale;
            newScale.x *= owner.transform.localScale.x / Mathf.Abs(owner.transform.localScale.x);
            data.gun.transform.localScale = newScale;
            
            data.gunRenderer.flipY = gunFacingLeft ? true : false;
           
            Vector3 shootPos = data.gun.transform.position + new Vector3(direction.x * data.gunWidth * 0.8f, direction.y * data.gunWidth * 0.8f, 0);
            Projectile.Create(owner.gameObject, projData, info.damage, Team.Friendly, shootPos, direction * info.speed, rotation, info.knockback);
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
