using System.Collections.Generic;
using UnityEngine;

namespace Game.Generation
{
    public class Level
    {
        public readonly Dictionary<Vector2Int, Room> rooms = new Dictionary<Vector2Int, Room>();
    }

}
