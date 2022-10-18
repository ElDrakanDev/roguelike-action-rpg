using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;

namespace Game.Entities
{
    public class ExampleEntity : Entity
    {
        [SerializeField] float shootFrequency = 2;
        [SerializeField] float shootCooldown = 2;
        [SerializeField] ProjectileDataSO projectile;

        protected override void AIUpdate()
        {
            shootCooldown -= Time.deltaTime;
            if(shootCooldown < 0)
            {
                shootCooldown = shootFrequency;
                UpdateAttackTarget();
                if(attackTarget != null)
                {
                    Vector3 shootDirection = (attackTargetPos - transform.position).normalized;
                    Projectile.Create(
                        gameObject,
                        projectile,
                        Stats.damage,
                        Team,
                        transform.position,
                        shootDirection,
                        0,
                        Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg
                    );
                }
            }
        }
    }
}
