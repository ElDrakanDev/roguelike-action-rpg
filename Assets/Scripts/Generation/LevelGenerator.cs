using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.ID;
using Game.RNG;
using Game.Helpers;
using System.Linq;

class RandomCompare : IComparer
{
    public int Compare(object x, object y)
    {
        return RNG.roomRng.Range(-1, 2);
    }
}

public class LevelGenerator
{
    Vector2Int _pos = Vector2Int.zero;
    List<Room> _availableRooms = new List<Room>();
    Level _level;

    public Level Generate(int normals, int shops, int treasures)
    {
        var newLevel = new Level();
        _level = newLevel;
        for (int i = 0; i < normals; i++) _availableRooms.Add(new Room(RoomType.Normal));
        for (int i = 0; i < shops; i++) _availableRooms.Add(new Room(RoomType.Shop));
        for (int i = 0; i < treasures; i++) _availableRooms.Add(new Room(RoomType.Treasure));

        _availableRooms = _availableRooms.OrderBy((room) => RNG.roomRng.Range(-1, 2)).ToList();

        CreateStartRoom();
        for(int i = _availableRooms.Count - 1; i >= 0; i--)
        {
            Room room = _availableRooms[i];
            Move();
            CreateRoom(room);
            _availableRooms.Remove(room);
        }
        CreateFinalRoom();
        return newLevel;
    }

    void Move()
    {
        var moveDir = Directions.directionVectors[EnumHelpers.GetRandom<MoveDirection>(RNG.roomRng)];
        _pos += moveDir;
        if (_level.rooms.ContainsKey(_pos)) Move();
    }

    void CreateRoom(Room room)
    {
        try
        {
            _level.rooms.Add(_pos, room);
            if (room.IsSpecial()) _pos = Vector2Int.zero;
        }
        catch(Exception ex)
        {
            Debug.LogError($"Se intentó crear una habitación en la posicion ya existente {_pos}. {ex}");
        }

    }
    void CreateStartRoom()
    {
        _level.rooms.Add(_pos, new Room(RoomType.Start));
    }
    void CreateFinalRoom()
    {
        Vector2Int[] positions = _level.rooms.Keys.ToArray();

        positions = positions.OrderByDescending(position => Vector2Int.Distance(Vector2Int.zero, position)).ToArray();

        var directions = EnumHelpers.Values<MoveDirection>();
        Array.Sort(directions, new RandomCompare());

        foreach (var position in positions)
        {
            foreach (MoveDirection direction in directions)
            {
                var newPosition = position + Directions.directionVectors[direction];

                if (CanSpawnBossRoomAt(newPosition))
                {
                    var room = new Room(RoomType.NextLevel);
                    _level.rooms.Add(newPosition, room);
                    return;
                }
            }
        }

        Debug.LogWarning($"No se pudo encontrar una posicion válida para crear la sala del jefe. Se creará ignorando las condiciones.");

        foreach (var position in positions)
        {
            foreach (MoveDirection direction in directions)
            {
                var newPosition = position + Directions.directionVectors[direction];

                if (!_level.rooms.ContainsKey(newPosition))
                {
                    var room = new Room(RoomType.NextLevel);
                    _level.rooms.Add(newPosition, room);
                    return;
                }
            }
        }
    }

    bool CanSpawnBossRoomAt(Vector2Int position)
    {
        if (_level.rooms.ContainsKey(position)) return false;

        var directions = EnumHelpers.Values<MoveDirection>();

        foreach (MoveDirection direction in directions)
        {
            var checkPosition = position + Directions.directionVectors[direction];
            _level.rooms.TryGetValue(checkPosition, out Room room);
            if (room == null) continue;
            if (room.IsSpecial()) return false;
        }
        return true;
    }
    /*bool CanSpawnRandom(Room newRoom){
        if (newRoom.type == RoomType.Start || newRoom.type == RoomType.Boss || newRoom.type == RoomType.NextLevel) return false;
        if (newRoom.IsSpecial() && _level.rooms.Count < minRooms / 2) return false;
        int shops = _level.rooms.Values.Count((room) => room.type == RoomType.Shop);
        int treasures = _level.rooms.Values.Count((room) => room.type == RoomType.Treasure);

        if (shops > 0 && newRoom.type == RoomType.Shop || treasures > 0 && newRoom.type == RoomType.Treasure)
            return false;
        return true;
    }*/
}
