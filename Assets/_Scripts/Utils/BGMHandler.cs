using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Game.Utils
{
    public class BGMHandler : MonoBehaviour
    {
        [SerializeField] BGMBySceneSO[] bgms;
        Scene _CurrentScene { get => SceneManager.GetActiveScene(); }
        bool bossWithThemeIsAlive = false;

        private void OnEnable()
        {
            EventManager.onRoomTypeChange += UpdateCurrentTrack;
        }
        void OnDisable()
        {
            EventManager.onRoomTypeChange += UpdateCurrentTrack;
        }
        private void Start()
        {
            
        }
        void UpdateCurrentTrack(int roomType)
        {
            if (!bossWithThemeIsAlive)
            {
                var bgm = bgms.Where(bgm => bgm.Scene == _CurrentScene).First();
                var clips = GetBgmClipByRoomType(bgm, roomType);
                BGMManager.Instance.Play(clips);
            }
        }

        AudioClip[] GetBgmClipByRoomType(BGMBySceneSO bgm, int roomType)
        {
            switch ((RoomType)roomType)
            {
                case RoomType.NextLevel:
                    return bgm.bossRoomClips;
                case RoomType.Shop:
                    return bgm.shopRoomClips;
                case RoomType.Boss:
                    return bgm.bossRoomClips;
                case RoomType.Treasure:
                    return bgm.treasureRoomClips;
                default:
                    return bgm.normalRoomClips;
            }
        }
    }
    public enum RoomType
    {
        Start, Normal, Treasure, NextLevel, Shop, Boss
    };
}
