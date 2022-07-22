using UnityEngine;
using System;

namespace Game.Helpers
{
    public static class EnumHelpers
    {
        public static T GetRandom<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}

