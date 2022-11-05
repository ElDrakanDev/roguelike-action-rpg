using Game.Unlocks;
using UnityEngine;

namespace Game.Players
{
    [CreateAssetMenu(menuName = "Players/Selection")]
    public class PlayerSelectSO : ScriptableObject
    {
        public string characterName;
        [TextArea(3, 10)]
        public string characterDescription;
        public Sprite sprite;
        public GameObject playerPrefab;
        [SerializeField] UnlockScriptableObject unlock;
        public bool Unlocked { get => unlock is not null ? unlock.IsUnlocked() : true; }
    }
}
