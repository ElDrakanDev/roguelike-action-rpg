using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Stats
{
    public class StatModifier : IStatComponent
    {
        float _value;
        public float Value
        {
            get { return _value; }
            set
            {
                try
                {
                    _value = value;
                    if(StatOwner != null)
                        StatOwner.needUpdate = true;
                }
                catch(NullReferenceException ex)
                {
                    Debug.LogError($"Se intentó actualizar el StatModifier pero no estaba atado a ningún contenedor.");
                    throw ex;
                }
            }
        }
        public StatType Type { get; set; }
        public Stat StatOwner { get; set; }
        public dynamic Source { get; set; }

        public StatModifier(float argValue, dynamic argSource, Stat argOwner, StatType argType)
        {
            Source = argSource;
            StatOwner = argOwner;
            Type = argType;
            Value = argValue;
        }
    }
}
