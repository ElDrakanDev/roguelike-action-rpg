using UnityEngine;
using Game.Generation;

namespace Game.Run
{
    [System.Serializable]
    public class RoomNavigator
    {
        Level _level;
        [SerializeField] Vector2Int _position = Vector2Int.zero;
        [SerializeField] Room _activeRoom;
        public Level CurrentLevel { get => _level; private set => _level = value; }
        public Room ActiveRoom { get { _activeRoom = CurrentLevel[Position] ; return CurrentLevel[Position]; } }
        public Vector2Int Position { get => _position; set =>  _position = value; }
        LevelGenerator generator;

        public RoomNavigator()
        {
            generator = new LevelGenerator();
        }

        public void Generate(int normals, int specials, int shops)
        {
            Position = Vector2Int.zero;
            _level = generator.Generate(normals, specials, shops);
        }

        public bool Move(int x, int y)
        {
            var newPos = Position + new Vector2Int(x, y);
            if(CurrentLevel.ContainsKey(newPos))
            {
                CurrentLevel.EnterRoom(Position, newPos);
                Position = newPos;
                Debug.Log($"Moved to: {Position}. RoomType: {ActiveRoom.Type}");
                return true;
            }
            return false;
        }
        public void MoveTo(int x, int y)
        {
            var newPos = new Vector2Int(x, y);
            CurrentLevel.EnterRoom(Position, newPos);
            Position = newPos;
        }
        public void MoveTo(Vector2Int pos)
        {
            CurrentLevel.EnterRoom(Position, pos);
            Position = pos;
        }
    }
}
