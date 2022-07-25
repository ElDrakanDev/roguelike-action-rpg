using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.ID;

namespace Game.Generation
{
    public class DrawLevel : MonoBehaviour
    {
        Level level;
        [SerializeField] int normalRooms = 10;
        [SerializeField] int shopRooms = 10;
        [SerializeField] int treasureRooms = 10;

        public GameObject square;
        private void Start()
        {
            LevelGenerator generator = new LevelGenerator();
            level = generator.Generate(normalRooms, shopRooms, treasureRooms);

            var renderer = square.GetComponent<SpriteRenderer>();

            foreach(var pos in level.rooms.Keys)
            {
                var room = level.rooms[pos];
                var newSquare = Instantiate(square, new Vector3(pos.x * renderer.bounds.size.x, pos.y * renderer.bounds.size.y, 0), Quaternion.identity);
                newSquare.GetComponent<SpriteRenderer>().color = GetColorByType(room.type);
                newSquare.name = room.type + pos.ToString();
            }
        }

        Color GetColorByType(RoomType type)
        {
            switch (type)
            {
                case RoomType.Treasure:
                    return Color.yellow;
                case RoomType.Shop:
                    return Color.magenta;
                case RoomType.NextLevel:
                    return Color.red;
                case RoomType.Start:
                    return Color.gray;
                default:
                    return Color.white;
            }
        }
    }
}
