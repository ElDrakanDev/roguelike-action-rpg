using UnityEngine;

namespace Game.Events
{
    public class EventManager : MonoBehaviour
    {
        static EventManager instance;
        public GameEvent leftClick = new GameEvent();

        private void Start()
        {
            if (!instance)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}
