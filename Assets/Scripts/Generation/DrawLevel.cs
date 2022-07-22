using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.ID;

public class DrawLevel : MonoBehaviour
{
    Level level;
    public int minRooms;
    public int minMoves;
    public int seed;
    public GameObject square;
    private void Start()
    {
        Random.InitState(seed);
        LevelGenerator generator = new LevelGenerator(minRooms, minMoves);
        level = generator.Generate();

        var renderer = square.GetComponent<SpriteRenderer>();

        foreach(var pos in level.rooms.Keys)
        {
            var room = level.rooms[pos];
            var newSquare = Instantiate(square, new Vector3(pos.x * renderer.bounds.size.x, pos.y * renderer.bounds.size.y, 0), Quaternion.identity);
            newSquare.GetComponent<SpriteRenderer>().color = GetColorByType(room.type);
            newSquare.name = pos.ToString();
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
