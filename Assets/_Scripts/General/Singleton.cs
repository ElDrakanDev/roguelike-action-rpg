using UnityEngine;

namespace Game.General
{
    public class Singleton : MonoBehaviour
    {
        public static Singleton instance;
        void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
