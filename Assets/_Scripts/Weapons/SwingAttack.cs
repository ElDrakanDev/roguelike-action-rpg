using UnityEngine;
using Game.Players;
using DG.Tweening;
using Game.Projectiles;
using Game.Utils;
using Game.Stats;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "New swing effect", menuName = "ScriptableObjects/Weapons/SwingEffect")]
    public class SwingAttack : WeaponAttack
    {
        enum SwingDirection { Forwards = 1, Backwards = -1}
        [SerializeField] ProjectileDataSO projData;
        [SerializeField] float margin = 0.5f;
        [SerializeField] float arc = 60f;
        [SerializeField] SwingDirection attackDirection = SwingDirection.Forwards;
        [SerializeField] float swingDuration = 0.2f;
        public override void Use(Player owner, Vector2 direction, WeaponStats stats)
        {
            Swing(owner, direction, stats);
        }
        public override void UseBegin(Player owner, Vector2 direction, WeaponStats stats)
        {
            Swing(owner, direction, stats);
        }
        public override void UseEnd(Player owner, Vector2 direction, WeaponStats stats) { }

        void Swing(Player owner, Vector2 direction, WeaponStats stats)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            bool flipY = rotation > 90 || rotation < -90 ? true : false;
            int swingDirection = (int)attackDirection;

            Projectile newProj = Projectile.Create(
                owner.gameObject, projData, stats.damage, Team.Friendly, new Vector3(999999, 999999, 100), direction, stats.knockback
            );
            Transform swordTransform = newProj.transform;
            swordTransform.SetParent(owner.transform);

            void SwingWeapon(float currentRotation)
            {
                if (swordTransform)
                {
                    swordTransform.position = owner.transform.position + new Vector3(
                        Mathf.Cos(currentRotation * Mathf.Deg2Rad) * margin,
                        Mathf.Sin(currentRotation * Mathf.Deg2Rad) * margin,
                        0
                    );
                    swordTransform.rotation = Quaternion.Euler(0, 0, currentRotation);
                }
            }

            if (flipY)
            {
                swordTransform.localScale = new Vector3(swordTransform.transform.localScale.x, -swordTransform.transform.localScale.y, 0);
                DOVirtual.Float(rotation - arc * swingDirection, rotation + arc * swingDirection, swingDuration, SwingWeapon)
                    .SetEase(Ease.OutFlash)
                    .OnComplete(() => { if(newProj) newProj.LifeTimeEnd(); });
                return;
            }
            DOVirtual.Float(rotation + arc * swingDirection, rotation - arc * swingDirection, swingDuration, SwingWeapon)
                .SetEase(Ease.OutFlash)
                .OnComplete(() => { if (newProj) newProj.LifeTimeEnd(); });
        }
    }
}