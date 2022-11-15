using UnityEngine;

namespace Game.Generation
{
    public enum ExploredState
    {
        NotDiscovered, Nearby, Explored
    }

    [System.Serializable]
    public class Room
    {
        [SerializeField] RoomType _type;
        public RoomType Type { get => _type; private set => _type = value; }
        public GameObject gameObject;
        public ExploredState exploredState = ExploredState.NotDiscovered;
        public readonly Vector2Int position;
        public static GameObject ActiveRoomGameObject { get; protected set; }
        public static Room ActiveRoom { get; protected set; }


        public Room(RoomType type, Vector2Int position)
        {
            Type = type;
            this.position = position;
        }

        public bool IsSpecial() => Type != RoomType.Normal;

        public static bool IsSpecial(RoomType type) => type != RoomType.Normal;

        public bool IsFixed()
        {
            return Type == RoomType.Start || Type == RoomType.Boss || Type == RoomType.NextLevel;
        }

        public static bool IsFixed(RoomType type)
        {
            return type == RoomType.Start || type == RoomType.Boss || type == RoomType.NextLevel;
        }

        public void Exit()
        {
            gameObject?.SetActive(false);
        }

        public void Enter()
        {
            ActiveRoomGameObject = gameObject;
            ActiveRoom = this;
            exploredState = ExploredState.Explored;
            gameObject?.SetActive(true);
        }
    }
}
