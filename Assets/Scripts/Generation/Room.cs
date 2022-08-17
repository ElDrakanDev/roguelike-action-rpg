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

        public Room(RoomType _type)
        {
            Type = _type;
        }

        public bool IsSpecial()
        {
            return Type != RoomType.Normal;
        }

        public bool IsFixed()
        {
            return Type == RoomType.Start || Type == RoomType.Boss || Type == RoomType.NextLevel;
        }

        public void Exit()
        {
            gameObject?.SetActive(false);
        }

        public void Enter()
        {
            exploredState = ExploredState.Explored;
            gameObject?.SetActive(true);
        }
    }
}
