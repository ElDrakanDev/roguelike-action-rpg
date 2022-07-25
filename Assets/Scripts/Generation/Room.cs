using Game.ID;
using UnityEngine;

namespace Game.Generation
{
    public class Room
    {
        public readonly RoomType type;
        public GameObject gameObject;

        public Room(RoomType _type)
        {
            type = _type;
        }

        public bool IsSpecial()
        {
            return type != RoomType.Normal;
        }

        public bool IsFixed()
        {
            return type == RoomType.Start || type == RoomType.Boss || type == RoomType.NextLevel;
        }
    }
}
