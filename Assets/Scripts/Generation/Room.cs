using Game.ID;
using UnityEngine;

public class Room
{
    public readonly RoomType type;
    public GameObject gameObject;

    public Room(RoomType _type)
    {
        type = _type;
    }

    public bool IsSpecial()
    {
        return type != RoomType.Normal;
    }
}
