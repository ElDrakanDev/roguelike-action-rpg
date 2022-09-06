using UnityEngine;
using Game.Interfaces;
using Game.Players;
using System.Collections.Generic;

namespace Game.Weapons
{
    public enum WeaponType { Melee, Ranged, Magic }

    [RequireComponent(typeof(Collider2D))]
    [System.Serializable]
    public class WeaponData : IWeapon
    {
        public string title = "Weapon Title";
        public string description = "Weapon Description";
        Vector2 aimDirection;
        [SerializeField] WeaponType type;
        [SerializeField] float damage = 1;
        [SerializeField] float useTime = 1;
        [SerializeField] float speed = 1;
        [SerializeField] bool autoUse = true;
        [HideInInspector] public Sprite sprite;
        [HideInInspector] public Player player;
        [HideInInspector] public GameObject owner;
        public List<WeaponAttack> attacks = new List<WeaponAttack>();
        float _cooldown = 0;
        bool _inUse = false;

        public WeaponData(string title, string description, float damage, float useTime, WeaponType type, Sprite sprite = null, GameObject owner = null)
        {
            this.owner = owner;
            this.title = title;
            this.description = description;
            this.sprite = sprite;
            this.damage = damage;
            this.type = type;
            this.useTime = useTime;

            if (owner) PickUp(owner);
        }
        public void PickUp(GameObject origin)
        {
            owner = origin;
            player = owner.GetComponent<Player>();
            player.weapon = this;
        }
        public void Drop()
        {
            if (player.weapon == this)
            {
                var newWeaponGameObject = new GameObject(title);
                newWeaponGameObject.transform.position = player.transform.position;
                var renderer = newWeaponGameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                var collider = newWeaponGameObject.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                var weaponContainer = newWeaponGameObject.AddComponent<WeaponContainer>();
                weaponContainer.data = this;
                newWeaponGameObject.layer = LayerMask.NameToLayer("Interactable");
                newWeaponGameObject.tag = "Interactable";
                player.weapon = null;
                owner = null;
            }
        }
        public void Update()
        {
            _cooldown -= Time.deltaTime;
        }
        public void Aim(Vector2 direction)
        {
            if(direction != Vector2.zero)
            {
                aimDirection = direction.normalized;
            }
        }
        public void UseBegin()
        {
            if(_cooldown < 0)
            {
                foreach (var effect in attacks)
                {
                    effect.UseBegin(player, aimDirection, damage, type, speed, useTime);
                }
                _cooldown = useTime;
                _inUse = true;
            }
        }
        public void Use()
        {
            if (autoUse && _cooldown < 0)
            {
                foreach (var effect in attacks)
                {
                    effect.Use(player, aimDirection, damage, type, speed, useTime);
                }
                _cooldown = useTime;
            }
        }
        public void UseEnd()
        {
            if (_inUse)
            {
                foreach(var effect in attacks)
                {
                    effect.UseEnd(player, aimDirection, damage, type, speed, useTime);
                }
            }
        }
    }
}
