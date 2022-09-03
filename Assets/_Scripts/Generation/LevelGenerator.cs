
namespace Game.Generation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
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
    class DistanceCompare : IComparer<Vector2Int>
    {
        readonly Vector2Int _center;
        readonly bool _closest;
        public DistanceCompare(Vector2Int _centerToMeasure, bool _closestPos = true)
        {
            _center = _centerToMeasure;
            _closest = _closestPos;
        }
        bool ByCompareMode(float xDist, float yDist)
        {
            if (_closest)
                return xDist < yDist;
            return xDist > yDist;
        }
        public int Compare(Vector2Int x, Vector2Int y)
        {
            var xDist = Vector2Int.Distance(x, _center);
            var yDist = Vector2Int.Distance(y, _center);

            if (ByCompareMode(xDist, yDist)) return -1;
            return 1;
        }
    }
    public class LevelGenerator
    {
        Vector2Int _pos = Vector2Int.zero;
        List<RoomType> _availableRooms = new List<RoomType>();
        Level _level;

        public Level Generate(int normals, int shops, int treasures)
        {
            var newLevel = new Level();
            _level = newLevel;
            for (int i = 0; i < treasures; i++) _availableRooms.Add(RoomType.Treasure);
            for (int i = 0; i < shops; i++) _availableRooms.Add(RoomType.Shop);
            for (int i = 0; i < normals; i++) _availableRooms.Add(RoomType.Normal);

            Shuffle(_availableRooms);

            CreateStartRoom();
            for (int i = _availableRooms.Count - 1; i >= 0; i--)
            {
                RoomType type = _availableRooms[i];
                if(!Room.IsSpecial(type)) Move();
                CreateRoom(type);
                _availableRooms.Remove(type);
            }
            CreateFinalRoom();
            return newLevel;
        }

        void Move()
        {
            var moveDir = Directions.directionVectors[EnumHelpers.GetRandom<MoveDirection>(RNG.roomRng)];
            _pos += moveDir;
            if (_level.ContainsKey(_pos))
            {
                Move();
            }
        }

        void CreateRoom(RoomType roomType)
        {
            try
            {
                if (Room.IsSpecial(roomType))
                {
                    var furthest = GetFurthestSpecialPosition();
                    _level.Add(furthest, new Room(roomType, furthest));
                    return;
                }
                _level.Add(_pos, new Room(roomType, _pos));
            }
            catch (ArgumentException ex)
            {
                Debug.LogError($"Se intentó crear una habitación en la posicion ya existente {_pos}. {ex}");
            }
        }
        void CreateStartRoom() => _level.Add(_pos, new Room(RoomType.Start, _pos));

        void CreateFinalRoom()
        {
            var furthest = GetFurthestSpecialPosition();
            var room = new Room(RoomType.NextLevel, furthest);
            _level.Add(furthest, room);               
        }

        bool CanSpawnSpecialRoomAt(Vector2Int position)
        {
            if (_level.ContainsKey(position)) return false;

            var directions = EnumHelpers.Values<MoveDirection>();

            foreach (MoveDirection direction in directions)
            {
                var checkPosition = position + Directions.directionVectors[direction];
                _level.TryGetValue(checkPosition, out Room room);
                if (room == null) continue;
                if (room.IsSpecial()) return false;
            }
            return true;
        }
        Vector2Int GetFurthestSpecialPosition()
        {
            Vector2Int[] positions = _level.Keys.ToArray();

            positions = positions.OrderByDescending(pos => pos, new DistanceCompare(Vector2Int.zero)).ToArray();

            var directions = EnumHelpers.Values<MoveDirection>();
            Array.Sort(directions, new RandomCompare());

            var finalPositions = new List<Vector2Int>();

            foreach (var position in positions)
            {
                foreach (MoveDirection direction in directions)
                {
                    finalPositions.Add(position + Directions.directionVectors[direction]);
                }
            }

            finalPositions.Sort(new DistanceCompare(Vector2Int.zero, false));

            foreach (var finalPos in finalPositions)
            {
                if (CanSpawnSpecialRoomAt(finalPos))
                {
                    return finalPos;
                }
            }
            Debug.LogWarning($"No se pudo encontrar una posicion válida para crear la sala especial. Se creará ignorando las condiciones.");

            foreach (var finalPos in finalPositions)
            {
                if (!_level.ContainsKey(finalPos))
                {
                    return finalPos;
                }
            }
            throw new KeyNotFoundException($"No se pudo encontrar una posición válida para la sala especial");
        }
        void Shuffle(List<RoomType> rooms)
        {
            for (int indexA = 0; indexA < rooms.Count; indexA++)
            {
                int indexB = RNG.roomRng.Range(0, rooms.Count);
                var roomA = rooms[indexA];
                var roomB = rooms[indexB];

                rooms[indexA] = roomB;
                rooms[indexB] = roomA;
            }
        }
    }
}
