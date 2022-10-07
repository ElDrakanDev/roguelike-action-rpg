using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    [CreateAssetMenu(menuName = "Projectiles/Data")]
    public class ProjectileDataSO : ScriptableObject
    {
        [SerializeField] GameObject _prefab;
        public GameObject Prefab { get => _prefab; }
        [SerializeField] ProjectileStatsSO _stats;
        public ProjectileStatsSO Stats { get => _stats; }
    }
}
