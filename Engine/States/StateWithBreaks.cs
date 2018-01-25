namespace Engine.States
{
    class StateWithBreaks
    { 
        private const uint maxTimeStamp = 50;
        protected uint timestamp;

        public StateWithBreaks()
        {
            timestamp = maxTimeStamp;
        }
        protected bool TickBreak()
        {
            if (timestamp < maxTimeStamp)
            {
                timestamp += 2;
                return false;
            }

            timestamp = 0;

            return true;
        }
    }
}
