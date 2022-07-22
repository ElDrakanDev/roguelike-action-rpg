using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.ID;
using Game.Helpers;
using System.Linq;

class RandomCompare : IComparer
{
    public int Compare(object x, object y)
    {
        return UnityEngine.Random.Range(-1, 2);
    }
}

public class LevelGenerator
{
    public readonly int minRooms;
    public readonly int minMoves;
    private int _moves;
    private Vector2Int _pos = Vector2Int.zero;
    Func<Level, Room, LevelGenerator, bool> canSpawnRandom = (level, newRoom, levelGen) => {
        if (newRoom.type == RoomType.Start || newRoom.type == RoomType.Boss || newRoom.type == RoomType.NextLevel) return false;
        if (newRoom.IsSpecial() && level.rooms.Count < levelGen.minRooms / 2) return false;
        int shops = level.rooms.Values.Count((room) => room.type == RoomType.Shop);
        int treasures = level.rooms.Values.Count((room) => room.type == RoomType.Treasure);

        if (shops > 0 && newRoom.type == RoomType.Shop || treasures > 0 && newRoom.type == RoomType.Treasure)
            return false;
        return true;
    };

    public LevelGenerator(int _minRooms, int _minMoves, Func<Level, Room, LevelGenerator, bool> spawnCondition = null)
    {
        minMoves = _minMoves;
        minRooms = _minRooms;
        canSpawnRandom = spawnCondition != null ? spawnCondition : canSpawnRandom;
    }

    public Level Generate()
    {
        var level = new Level();

        CreateStartRoom(level);
        while(level.rooms.Values.Count < minRooms || _moves < minMoves)
        {
            Move();
            CreateRandomRoomIfNull(level);
        }
        CreateFinalRoom(level);
        return level;
    }

    void Move()
    {
        var moveDir = Directions.directionVectors[EnumHelpers.GetRandom<MoveDirection>()];
        _pos += moveDir;
        _moves++;
    }

    void CreateRandomRoomIfNull(Level level)
    {
        if (!level.rooms.ContainsKey(_pos))
        {
            RoomType type = EnumHelpers.GetRandom<RoomType>();
            Room room = new Room(type);
            if (canSpawnRandom(level, room, this))
                level.rooms.Add(_pos, room);
            else
                CreateRandomRoomIfNull(level);
        }
    }
    void CreateStartRoom(Level level)
    {
        level.rooms.Add(_pos, new Room(RoomType.Start));
    }
    void CreateFinalRoom(Level level)
    {
        Vector2Int[] positions = level.rooms.Keys.ToArray();

        positions = positions.OrderByDescending(position => Vector2Int.Distance(Vector2Int.zero, position)).ToArray();

        var directions = EnumHelpers.Values<MoveDirection>();
        Array.Sort(directions, new RandomCompare());

        foreach (var position in positions)
        {
            foreach (MoveDirection direction in directions)
            {
                var newPosition = position + Directions.directionVectors[direction];
                if(!level.rooms.ContainsKey(newPosition))
                {
                    var room = new Room(RoomType.NextLevel);
                    level.rooms.Add(newPosition, room);
                    return;
                }
            }
        }
    }
}
