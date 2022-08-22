using UnityEngine;

namespace Game.Stats
{
    [CreateAssetMenu(fileName = "NewBaseStats", menuName = "ScriptableObjects/Stats/BaseStatObject", order = 1)]
    public class BaseStatObject : ScriptableObject
    {
        public BaseStatValue[] baseStats;
    }
}