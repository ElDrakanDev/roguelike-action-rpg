using System.Collections.Generic;
using UnityEngine;

namespace Game.ID
{
    public enum RoomType
    {
        Normal, Treasure, NextLevel, Shop, Boss
    };

    public enum MoveDirection
    {
        Up, Right, Left, Down
    }

    public static class Directions
    {
        public static Dictionary<MoveDirection, Vector2Int> directionVectors = new Dictionary<MoveDirection, Vector2Int>()
        {
            {MoveDirection.Up, new Vector2Int(0, 1) },
            {MoveDirection.Right, new Vector2Int(1, 0) },
            {MoveDirection.Down, new Vector2Int(0, -1) },
            {MoveDirection.Left, new Vector2Int(-1, 0) }
        };
    }
}

