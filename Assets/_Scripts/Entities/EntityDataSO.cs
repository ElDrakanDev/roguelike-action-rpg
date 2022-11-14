using UnityEngine;

namespace Game.Entities
{
    [CreateAssetMenu(menuName = "Entities/Data")]
    public class EntityDataSO : ScriptableObject
    {
        [SerializeField] GameObject _prefab;
        public GameObject Prefab { get => _prefab; }
        [SerializeField] EntityStatsSO _stats;
        public EntityStatsSO Stats { get => _stats; }
        public AudioClip hitEffect;
        public AudioClip deathEffect;
    }
}
