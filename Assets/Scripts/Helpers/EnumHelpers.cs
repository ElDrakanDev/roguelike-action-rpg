using System;

namespace Game.Helpers
{
    using RNG = Game.RNG.RNG;

    public static class EnumHelpers
    {
        public static T GetRandom<T>(RNG rng=null)
        {
            Array values = Enum.GetValues(typeof(T));
            if(rng != null)
                return (T)values.GetValue(rng.Range(0, values.Length));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        public static Array Values<T>()
        {
            return Enum.GetValues(typeof(T));
        }
    }
}

