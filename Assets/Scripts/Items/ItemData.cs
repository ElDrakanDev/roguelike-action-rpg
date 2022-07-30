using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Events;
using Game.Stats;
using Game.Effects;

namespace Game.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemData : ScriptableObject
    {
        [SerializeField] string _id;
        public string ID { get => _id; }
        public StatType[] types;
        public float[] amounts;
        public GameEvent[] events;
        public Effect[] effects;
        public Sprite sprite;
        public string description;
    }
}
