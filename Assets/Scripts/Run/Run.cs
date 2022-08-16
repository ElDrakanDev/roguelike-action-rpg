
namespace Game.Run
{
    using UnityEngine;
    using Game.RNG;
    using UnityEngine.InputSystem;

    public class Run : MonoBehaviour
    {
        [SerializeField] int _seed;
        public int Seed { get => _seed; private set => _seed = value; }
        public static Run instance;
        public RoomNavigator navigator;

        public int normals = 8, specials = 1, shops = 1;

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
            navigator = new RoomNavigator();
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            navigator.Generate(normals, specials, shops);
        }

        private void Update()
        {
            Keyboard kb = Keyboard.current;
            if (kb.leftArrowKey.wasPressedThisFrame) navigator.Move(-1, 0);
            else if (kb.upArrowKey.wasPressedThisFrame) navigator.Move(0, 1);
            else if (kb.rightArrowKey.wasPressedThisFrame) navigator.Move(1, 0);
            else if (kb.downArrowKey.wasPressedThisFrame) navigator.Move(0, -1);
        }
    }
}
