using UnityEngine;
using Game.Players;
using DG.Tweening;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "New swing effect", menuName = "ScriptableObjects/Weapons/SwingEffect")]
    public class SwingAttack : WeaponAttack
    {
        [SerializeField] GameObject sword;
        [SerializeField] float margin = 0.5f;
        [SerializeField] float arc = 60f;

        public override void Use(Player owner, Vector2 direction, float damage, WeaponType type, float speed, float useTime)
        {
            Swing(owner, direction, damage, type, speed, useTime);
        }
        public override void UseBegin(Player owner, Vector2 direction, float damage, WeaponType type, float speed, float useTime)
        {
            Swing(owner, direction, damage, type, speed, useTime);
        }
        public override void UseEnd(Player owner, Vector2 direction, float damage, WeaponType type, float speed, float useTime) { }

        void Swing(Player owner, Vector2 direction, float damage, WeaponType type, float speed, float useTime)
        {
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            bool flipY = rotation > 90 || rotation < -90 ? true : false;
            
            var newSword = Instantiate(sword);
            newSword.transform.SetParent(owner.transform);
            newSword.transform.position = owner.transform.position + new Vector3(direction.x * margin, direction.y * margin, 0) * margin;

            void SwingWeapon(float currentRotation)
            {
                newSword.transform.position = owner.transform.position + new Vector3(Mathf.Cos(currentRotation * Mathf.Deg2Rad) * margin, Mathf.Sin(currentRotation * Mathf.Deg2Rad) * margin, 0);
                newSword.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            }
            if (flipY)
            {
                newSword.transform.localScale = new Vector3(newSword.transform.localScale.x, -newSword.transform.localScale.y, 0);
                DOVirtual.Float(rotation - arc, rotation + arc, useTime, SwingWeapon).SetEase(Ease.OutFlash).OnComplete(() => Destroy(newSword));
                return;
            }
            DOVirtual.Float(rotation + arc, rotation - arc, useTime, SwingWeapon).SetEase(Ease.OutFlash).OnComplete(() => Destroy(newSword));
        }
    }
}