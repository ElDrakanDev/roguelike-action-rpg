using System;

namespace Game.RNG
{
    public class RNG
    {
        Random _rand;
        ulong _state = 0;
        public ulong State { get => _state; }
        public static RNG itemRng;
        public static RNG roomRng;
        public RNG(int seed, ulong state = 1)
        {
            _rand = new Random(seed);
            for (ulong i = 0; i < state - 1; i++)
            {
                _state++;
                _rand.Next(0, 2);
            }
        }

        public static void Initialize(int seed)
        {
            itemRng = new RNG(seed);
            roomRng = new RNG(seed);
        }

        public int Range(int min, int exMax)
        {
            _state++;
            return _rand.Next(min, exMax);
        }

        public float FRange(float min, float max)
        {
            _state++;
            return ((float)_rand.NextDouble() + min) * max;
        }
    }
}
