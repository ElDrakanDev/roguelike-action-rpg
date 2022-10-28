using UnityEngine;
using Game.Interfaces;
using Game.Players;
using Game.Stats;
using Game.Utils;

namespace Game.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        const float MIN_ATTACK_SPEED_MULTIPLIER = 0.1f;
        const float MIN_INACCURACY_DIVISOR = 0.1f;

        Vector2 _aimDirection;
        public WeaponStats stats;
        [SerializeField] WeaponDataSO weaponData;
        public Player Owner { get => _owner; protected set => _owner = value; }
        Player _owner;
        WeaponAttackMode attackMode;
        float _cooldown = 0;
        bool _inUse = false;
        WeaponAttack[] Attacks { get => weaponData.attacks; }
        
        public float FlatBonus { get => Owner.stats.StatTotal(stats.flatAttributes); }
        public float Multiplier { get => Owner.stats.StatTotal(stats.multAttributes); }
        public float Damage {
            get
            {
                float total = (stats.damage + FlatBonus) * Multiplier;
                if (total < 1) return 1;
                return total;
            }
            set => stats.damage = value;
        }
        public float AttackSpeedMultiplier
        {
            get
            {
                float multiplier = Owner is not null ? Owner.stats[AttributeID.Agility].Value : 0;
                if (multiplier < MIN_ATTACK_SPEED_MULTIPLIER) return MIN_ATTACK_SPEED_MULTIPLIER;
                return multiplier;
            }
        }
        public float UseTime { get => stats.useTime / AttackSpeedMultiplier; set => stats.useTime = value; }
        float Inaccuracy {
            get
            {
                float divisor = Owner is not null ? Owner.stats[AttributeID.Accuracy].Value : 0;
                if (divisor <= MIN_INACCURACY_DIVISOR) divisor = MIN_INACCURACY_DIVISOR;
                return stats.inaccuracy / divisor;
            }
        }

        public Vector2 AimDirection {
            get
            {
                float angle = _aimDirection.Angle();
                float inaccuracy = Inaccuracy;
                angle += Random.Range(-inaccuracy, inaccuracy);
                return Vector2Extension.DirectionFromAngle(angle);
            }
        }

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
                _aimDirection = direction.normalized;
            }
        }
        public void UseBegin()
        {
            if(_cooldown < 0)
            {
                foreach (var attack in attackMode.ChooseAttacks(Attacks))
                {
                    attack.UseBegin(Owner, AimDirection, stats);
                }
                _cooldown = UseTime;
                _inUse = true;
            }
        }
        public void Use()
        {
            if (stats.autoUse && _cooldown < 0)
            {
                foreach (var attack in attackMode.ChooseAttacks(Attacks))
                {
                    attack.Use(Owner, AimDirection, stats);
                }
                _cooldown = UseTime;
            }
        }
        public void UseEnd()
        {
            if (_inUse)
            {
                foreach(var attack in attackMode.ChooseAttacks(Attacks, true))
                {
                    attack.UseEnd(Owner, AimDirection, stats);
                }
            }
        }
    }
}
