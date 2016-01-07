namespace Engine.States
{
    class StateWithBreaks
    { 
        private const uint maxTimeStamp = 50;
        protected uint timestamp;

        public StateWithBreaks()
        {
            this.timestamp = maxTimeStamp;
        }
        protected bool TickBreak()
        {
            if (this.timestamp < maxTimeStamp)
            {
                this.timestamp += 2;
                return false;
            }

            this.timestamp = 0;

            return true;
        }
    }
}
