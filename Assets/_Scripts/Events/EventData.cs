namespace Game.Events
{
    public class EventData
    {
        public float amount;
        public dynamic source;
        public dynamic destination;
        public dynamic type;

        public EventData(float amount = 0, dynamic source = null, dynamic destination = null, dynamic type = null)
        {
            this.amount = amount;
            this.source = source;
            this.destination = destination;
            this.type = type;
        }
    }
}
