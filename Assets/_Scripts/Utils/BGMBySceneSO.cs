using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Events;

namespace Game.Utils
{
    [CreateAssetMenu(menuName = "Utils/SceneDefaultBGM")]
    public class BGMBySceneSO : ScriptableObject
    {
        [field:SerializeField] string _SceneName { get; set; }
        public Scene Scene { get => SceneManager.GetSceneByName(_SceneName); }
        [field:SerializeField] public AudioClip[] normalRoomClips;
        [field: SerializeField] public AudioClip[] shopRoomClips;
        [field: SerializeField] public AudioClip[] treasureRoomClips;
        [field: SerializeField] public AudioClip[] bossRoomClips;
    }
    
    [System.Serializable]
    public struct BGMData
    {
        public AudioClip[] clips;
        public RoomType roomType;
        public bool easeIn;

        public BGMData(AudioClip[] clips, RoomType roomType, bool easeIn)
        {
            this.clips = clips;
            this.roomType = roomType;
            this.easeIn = easeIn;
        }
    }
}
