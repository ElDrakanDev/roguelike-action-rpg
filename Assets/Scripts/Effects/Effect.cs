using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Effects
{
    //[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Effects/_")]
    public abstract class Effect : ScriptableObject
    {
        public virtual void Run(EventData data=null) { }
        public virtual void Kill(EventData data=null) { }
    }
}
