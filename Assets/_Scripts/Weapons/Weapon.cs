using UnityEngine;
using Game.Interfaces;
using Game.Players;

namespace Game.Weapons
{
    public enum WeaponType { Melee, Ranged, Magic }

    [System.Serializable]
    public class Weapon : IWeapon
    {
        public string title = "Weapon Title";
        public string description = "Weapon Description";
        Vector2 aimDirection;
        public WeaponStats stats;
        [HideInInspector] public GameObject containerPrefab;
        [HideInInspector] public Sprite sprite;
        [HideInInspector] public Player player;
        [HideInInspector] public GameObject owner;
        public WeaponAttack[] attacks;
        WeaponAttackMode attackMode;
        float _cooldown = 0;
        bool _inUse = false;

        public Weapon(string title, string description, Sprite sprite = null, GameObject owner = null)
        {
            this.owner = owner;
            this.title = title;
            this.description = description;
            this.sprite = sprite;
            attackMode = WeaponAttackMode.FromEnum(stats.attackMode);

            if (owner) PickUp(owner);
        }
        public void PickUp(GameObject origin)
        {
            attackMode = WeaponAttackMode.FromEnum(stats.attackMode);
            owner = origin;
            player = owner.GetComponent<Player>();
            player.weapon = this;
        }
        public void Drop()
        {
            if (player.weapon == this)
            {
                Vector3 randomRotation = new Vector3(0, 0, Random.Range(0, 361));
                GameObject.Instantiate(containerPrefab, player.transform.position, Quaternion.Euler(randomRotation));
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
                foreach (var attack in attackMode.ChooseAttacks(attacks))
                {
                    attack.UseBegin(player, aimDirection, stats);
                }
                _cooldown = stats.useTime;
                _inUse = true;
            }
        }
        public void Use()
        {
            if (stats.autoUse && _cooldown < 0)
            {
                foreach (var attack in attackMode.ChooseAttacks(attacks))
                {
                    attack.Use(player, aimDirection, stats);
                }
                _cooldown = stats.useTime;
            }
        }
        public void UseEnd()
        {
            if (_inUse)
            {
                foreach(var attack in attackMode.ChooseAttacks(attacks, true))
                {
                    attack.UseEnd(player, aimDirection, stats);
                }
            }
        }
    }
}
