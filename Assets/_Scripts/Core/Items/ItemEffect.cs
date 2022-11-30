using UnityEngine;

// [CreateAssetMenu(fileName = "default effect", menuName = "dropdown category/name", order = 1)]
namespace Game.Items
{
    public abstract class ItemEffect : ScriptableObject
    {
        public abstract void Apply(GameObject owner);
        public abstract void Remove(GameObject owner);
    }
}
