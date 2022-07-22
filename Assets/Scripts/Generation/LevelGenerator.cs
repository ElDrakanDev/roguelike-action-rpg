using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.ID;
using Game.Helpers;

public class LevelGenerator
{
    public readonly int minRooms;
    public readonly int minMoves;
    private int _moves;
    private Vector2Int _pos = Vector2Int.zero;

    public LevelGenerator(int _minRooms, int _minMoves)
    {
        minMoves = _minMoves;
        minRooms = _minRooms;
    }

    public Level Generate()
    {
        var level = new Level();

        level.rooms.Add(_pos, new Room(RoomType.Normal));
        while(level.rooms.Values.Count < minRooms || _moves < minMoves)
        {
            Move();
            CreateRoomIfNull(level);
        }

        return level;
    }

    void Move()
    {
        var moveDir = Directions.directionVectors[EnumHelpers.GetRandom<MoveDirection>()];
        _pos += moveDir;
        _moves++;
    }

    void CreateRoomIfNull(Level level)
    {
        if (!level.rooms.ContainsKey(_pos))
        {
            RoomType type = EnumHelpers.GetRandom<RoomType>();
            Room room = new Room(type);
            level.rooms.Add(_pos, room);
        }
    }
}
