using Game.ID;
using UnityEngine;

public class Room
{
    public readonly RoomType type;
    public readonly Vector2Int pos;
    public GameObject gameObject;

    public Room(RoomType _type, Vector2Int _pos)
    {
        type = _type;
        pos = _pos;
    }
}
