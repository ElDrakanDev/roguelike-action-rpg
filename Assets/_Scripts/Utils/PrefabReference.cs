using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    [CreateAssetMenu(menuName = "Utils/PrefabReference")]
    public class PrefabReference : ScriptableObject
    {
        public GameObject prefab;
    }
}
