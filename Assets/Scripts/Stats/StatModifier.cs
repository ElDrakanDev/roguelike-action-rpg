using Game.ID;
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
                    Owner.needUpdate = true;
                }
                catch(NullReferenceException ex)
                {
                    Debug.LogError($"Se intentó actualizar el StatModifier pero no estaba atado a ningún contenedor.");
                    throw ex;
                }
            }
        }
        public StatType Type { get; set; }
        public Stat Owner { get; set; }
        public object Source { get; set; }

        public StatModifier(float argValue, object argSource, Stat argOwner, StatType argType)
        {
            Source = argSource;
            Owner = argOwner;
            Type = argType;
            Value = argValue;
        }
    }
}
