using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Generation
{
    public class Level : Dictionary<Vector2Int, Room>
    {
        public void EnterRoom(Vector2Int toEnter)
        {
            foreach (Vector2Int direction in Directions.directionVectors.Values)
            {
                var checkPos = toEnter + direction;

                if (this.ContainsKey(checkPos) && this[checkPos].exploredState is ExploredState.NotDiscovered)
                {
                    this[checkPos].exploredState = ExploredState.Nearby;
                }
            }

            this[toEnter].Enter();
            EventManager.OnRoomChange();
            EventManager.OnRoomTypeChange((int)this[toEnter].Type);
        }
        public void EnterRoom(Vector2Int previous, Vector2Int toEnter)
        {
            this[previous].Exit();

            foreach(Vector2Int direction in Directions.directionVectors.Values)
            {
                var checkPos = toEnter + direction;

                if (this.ContainsKey(checkPos) && this[checkPos].exploredState is ExploredState.NotDiscovered)
                {
                    this[checkPos].exploredState = ExploredState.Nearby;
                }
            }

            this[toEnter].Enter();
            EventManager.OnRoomChange();
            EventManager.OnRoomTypeChange((int)this[toEnter].Type);
        }
    }

}
