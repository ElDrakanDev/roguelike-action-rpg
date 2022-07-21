using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public readonly int minRooms;
    public readonly int minMoves;
    public readonly Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();

    public Level(int _minRooms, int _minMoves)
    {
        minMoves = _minMoves;
        minRooms = _minRooms;
    }
}
