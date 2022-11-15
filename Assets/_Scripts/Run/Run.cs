
namespace Game.Run
{
    using UnityEngine;
    using Game.RNG;
    using Game.Generation;
    using Game.Events;
    using Game.Unlocks;

    public class Run : MonoBehaviour
    {
        [SerializeField] int _seed;
        [HideInInspector] public static Run instance;
        [SerializeField] UnlockManager _unlockManager;
        RoomContainer _roomContainer;
        public RoomNavigator navigator;
        public int Seed { get => _seed; private set => _seed = value; }
        public Level Level{ get => navigator.CurrentLevel; }
        public Room ActiveRoom { get => navigator.ActiveRoom; }
        RoomContainer LevelContainer {
            get
            {
                if (!_roomContainer)
                    _roomContainer = FindObjectOfType<RoomContainer>();
                return _roomContainer;
            }
            set => _roomContainer = value;
        }

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
            _unlockManager = new UnlockManager();
        }
        private void Start()
        {
            Initialize(Seed == 0 ? Random.Range(int.MinValue, int.MaxValue) : Seed);
        }
        public void Initialize(int seed)
        {
            Debug.Log($"Inicializando partida con la semilla {seed}");
            Seed = seed;
            RNG.Initialize(Seed);
            navigator = new RoomNavigator(new Game.Input.PlayerActionsControls());
            LevelContainer.GenerateLevel();
            navigator.CurrentLevel = LevelContainer.CurrentLevel;
            EventManager.OnRoomChange();
        }
    }
}
