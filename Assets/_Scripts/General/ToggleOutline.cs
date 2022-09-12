using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.General
{
    [CreateAssetMenu(menuName = "Utils/Outline")]
    public class ToggleOutline : ScriptableObject
    {
        [SerializeField] public Material normalMaterial;
        [SerializeField] public Material hoverMaterial;
    }
}
