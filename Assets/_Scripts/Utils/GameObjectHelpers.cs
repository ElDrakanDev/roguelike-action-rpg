using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class GameObjectHelpers
    {
        public static GameObject Closest(this IEnumerable<GameObject> gameObjects, Vector3 compareTo)
        {
            GameObject closest = null;
            float minDistance = float.MaxValue;
            foreach (var gameObject in gameObjects)
            {
                float dist = Vector3.Distance(compareTo, gameObject.transform.position);
                if (dist < minDistance)
                {
                    closest = gameObject;
                    minDistance = dist;
                }
            }
            return closest;
        }
    }
}
