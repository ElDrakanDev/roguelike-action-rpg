using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Generation
{
    [CreateAssetMenu(fileName = "New Room Layout", menuName = "RoomLayouts/RoomLayout")]
    public class RoomLayoutsContainer : ScriptableObject
    {
        [SerializeField] GameObject[] startRooms;
        [SerializeField] GameObject[] normalRooms;
        [SerializeField] GameObject[] shopRooms;
        [SerializeField] GameObject[] treasureRooms;
        [SerializeField] GameObject[] nextRooms;
        [SerializeField] GameObject[] bossRooms;

        Dictionary<RoomType, GameObject[]> roomsDict;
        
        private void OnEnable()
        {
            roomsDict = new Dictionary<RoomType, GameObject[]>()
            {
                {RoomType.Start, startRooms},
                {RoomType.Normal, normalRooms},
                {RoomType.Shop, shopRooms},
                {RoomType.Treasure, treasureRooms},
                {RoomType.NextLevel, nextRooms},
                {RoomType.Boss, bossRooms}
            };
        }

        public GameObject[] this[RoomType type]
        {
            get
            {
                try
                {
                    return roomsDict[type];
                }
                catch(NullReferenceException ex)
                {
                    Debug.LogError($"Se intentó obtener el array correspondiente a {type}, pero no existía.");
                    throw ex;
                }
            }
        }
            
    }
}
