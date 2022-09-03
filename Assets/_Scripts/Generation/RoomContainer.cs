using UnityEngine;
using Game.RNG;

namespace Game.Generation
{
    public class RoomContainer : MonoBehaviour
    {
        [HideInInspector] public RoomContainer instance;
        [SerializeField] RoomLayoutsContainer layouts;
        Level _level;
        public Level CurrentLevel { get => _level; private set => _level = value; }
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
        }

        public void GenerateLevel()
        {
            LevelGenerator generator = new LevelGenerator();
            CurrentLevel = generator.Generate(normals, shops, specials); 
            foreach (var pos in CurrentLevel.Keys)
            {
                var room = CurrentLevel[pos];
                var layout = layouts[room.Type];
                var roomPrefab = layout[RNG.RNG.roomRng.Range(0, layout.Length)];
                var newRoom = Instantiate(roomPrefab, transform.position, Quaternion.identity);
                newRoom.GetComponent<TestRoomContainer>().room = room;
                newRoom.name = room.Type + pos.ToString();
                room.gameObject = newRoom;
                newRoom.SetActive(false);
            }
        }
    }
}
