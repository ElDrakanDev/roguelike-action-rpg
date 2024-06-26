using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public class Lifetime : MonoBehaviour
    {
        public float lifetime;
        void Update()
        {
            if (lifetime < 0) Destroy(gameObject);
            lifetime -= Time.deltaTime;
        }
    }
}
