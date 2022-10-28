using UnityEngine;

namespace Game.Utils
{
    public static class Vector2Extension
    {
        public static float Angle(this Vector2 v) => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        public static float AngleFromDirection(Vector2 v) => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        public static Vector2 DirectionFromAngle(float angle)
        {
            return new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
        }
    }
}
