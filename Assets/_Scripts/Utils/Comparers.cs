using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class VectorIntDistanceCompare : IComparer<Vector2Int> 
    {
        readonly Vector2Int _center;
        readonly bool _closest;
        public VectorIntDistanceCompare(Vector2Int _centerToMeasure, bool _closestPos = true)
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
    public class Vector3DistanceCompare : IComparer<Vector3>
    {
        readonly Vector3 _center;
        readonly bool _closest;
        public Vector3DistanceCompare(Vector3 _centerToMeasure, bool _closestPos = true)
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
        public int Compare(Vector3 x, Vector3 y)
        {
            var xDist = Vector3.Distance(x, _center);
            var yDist = Vector3.Distance(y, _center);

            if (ByCompareMode(xDist, yDist)) return -1;
            return 1;
        }
    }
}