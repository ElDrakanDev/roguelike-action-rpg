namespace Game.Stats
{
    [System.Serializable]
    public struct ItemStatValue
    {
        public StatType type;
        public AttributeID attribute;
        public float value;

        public ItemStatValue(StatType type, AttributeID attribute, float value)
        {
            this.type = type;
            this.attribute = attribute;
            this.value = value;
        }
    }

    [System.Serializable]
    public struct BaseStatValue
    {
        public AttributeID attribute;
        public float baseValue;
        public float minValue;
        public float maxValue;

        public BaseStatValue(AttributeID attribute, float baseValue, float minValue, float maxValue)
        {
            this.attribute = attribute;
            this.baseValue = baseValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}
