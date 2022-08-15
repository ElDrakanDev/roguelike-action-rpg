
namespace Game.Run
{
    using UnityEngine;
    using Game.RNG;
    using Game.Generation;

    public class Run : MonoBehaviour
    {
        [SerializeField] int _seed;
        public int Seed { get => _seed; private set => _seed = value; }
        public static Run instance;

        void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Initialize(Seed == 0 ? Random.Range(int.MinValue, int.MaxValue) : Seed);
        }

        void Initialize(int seed)
        {
            Seed = seed;
            RNG.Initialize(Seed);
        }
    }
}
