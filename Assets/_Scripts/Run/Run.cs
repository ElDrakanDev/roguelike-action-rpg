
namespace Game.Run
{
    using UnityEngine;
    using Game.RNG;
    using UnityEngine.InputSystem;
    using Game.Generation;

    public class Run : MonoBehaviour
    {
        [SerializeField] int _seed;
        [SerializeField] GameObject roomPrefab;
        public int Seed { get => _seed; private set => _seed = value; }
        public static Run instance;
        public RoomNavigator navigator;
        public int normals = 8, specials = 1, shops = 1;
        public Level Level{ get => navigator.CurrentLevel; }
        public Room ActiveRoom { get => navigator.ActiveRoom; }

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

        }
        private void Start()
        {
            Initialize(Seed == 0 ? Random.Range(int.MinValue, int.MaxValue) : Seed);
        }
        void Initialize(int seed)
        {
            Seed = seed;
            RNG.Initialize(Seed);
            navigator = new RoomNavigator(new Game.Input.PlayerActionsControls());
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            navigator.Generate(normals, specials, shops);
            foreach (var pos in navigator.CurrentLevel.Keys)
            {
                var room = navigator.CurrentLevel[pos];
                var newSquare = Instantiate(roomPrefab, transform.position, Quaternion.identity);
                newSquare.GetComponent<SpriteRenderer>().color = GetColorByType(room.Type);
                newSquare.GetComponent<TestRoomContainer>().room = room;
                newSquare.name = room.Type + pos.ToString();
                room.gameObject = newSquare;
                newSquare.SetActive(false);
            }
            navigator.CurrentLevel.EnterRoom(Vector2Int.zero);
        }

        private void Update()
        {
            Keyboard kb = Keyboard.current;
            if (kb.leftArrowKey.wasPressedThisFrame) navigator.Move(-1, 0);
            else if (kb.upArrowKey.wasPressedThisFrame) navigator.Move(0, 1);
            else if (kb.rightArrowKey.wasPressedThisFrame) navigator.Move(1, 0);
            else if (kb.downArrowKey.wasPressedThisFrame) navigator.Move(0, -1);
        }

        public void ReadInputDirection(InputAction.CallbackContext context) => navigator.ReadInputDirection(context);
        public void ReadInputCancel(InputAction.CallbackContext context) => navigator.ReadInputCancel(context);
        Color GetColorByType(RoomType type)
        {
            switch (type)
            {
                case RoomType.Treasure:
                    return Color.yellow;
                case RoomType.Shop:
                    return Color.magenta;
                case RoomType.NextLevel:
                    return Color.red;
                case RoomType.Start:
                    return Color.gray;
                default:
                    return Color.white;
            }
        }
    }
}
