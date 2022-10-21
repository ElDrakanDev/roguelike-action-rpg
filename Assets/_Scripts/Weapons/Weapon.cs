using UnityEngine;
using Game.Interfaces;
using Game.Players;
using Game.Stats;

namespace Game.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        Vector2 aimDirection;
        public WeaponStats stats;
        [SerializeField] WeaponDataSO weaponData;
        public Player Owner { get => _owner; protected set => _owner = value; }
        Player _owner;
        WeaponAttackMode attackMode;
        float _cooldown = 0;
        bool _inUse = false;
        WeaponAttack[] Attacks { get => weaponData.attacks; }

        void Awake()
        {
            if(TryGetComponent(out _owner) && weaponData is not null)
            {
                Initialize(weaponData);
            }
        }

        /// <summary>
        /// Inicializa el arma y sus datos, asumiendo que fue agregado a un jugador
        /// </summary>
        /// <param name="weaponData">Los datos del arma agarrada</param>
        public void Initialize(WeaponDataSO weaponData)
        {
            this.weaponData = weaponData;
            stats = weaponData.statsScriptable.CreateStats();
            attackMode = WeaponAttackMode.FromEnum(weaponData.attackMode);
            Owner = gameObject.GetComponent<Player>();
            Owner.weapon?.Drop();
            Owner.weapon = this;
        }

        public void Drop(bool spawnPickable = true)
        {
            if (spawnPickable)
            {
                Vector3 randomRotation = new Vector3(0, 0, Random.Range(0, 361));
                Instantiate(weaponData.pickable, transform.position, Quaternion.Euler(randomRotation), Run.Run.instance.ActiveRoom.gameObject.transform);
            }
            Destroy(this);
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
                foreach (var attack in attackMode.ChooseAttacks(Attacks))
                {
                    attack.UseBegin(Owner, aimDirection, stats);
                }
                _cooldown = stats.useTime;
                _inUse = true;
            }
        }
        public void Use()
        {
            if (stats.autoUse && _cooldown < 0)
            {
                foreach (var attack in attackMode.ChooseAttacks(Attacks))
                {
                    attack.Use(Owner, aimDirection, stats);
                }
                _cooldown = stats.useTime;
            }
        }
        public void UseEnd()
        {
            if (_inUse)
            {
                foreach(var attack in attackMode.ChooseAttacks(Attacks, true))
                {
                    attack.UseEnd(Owner, aimDirection, stats);
                }
            }
        }
    }
}
