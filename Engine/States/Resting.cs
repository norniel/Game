namespace Engine.States
{
    class Resting : IState
    {
        protected readonly MobileObject _mobileObject;
        private const int MAX_MAXRESTINGTICK = 20;
        private int _restingTicks = 0;
        private readonly int _maxRestingTicks;
        public Resting(MobileObject mobileObject)
        {
            _mobileObject = mobileObject;
            _maxRestingTicks = Game.Random.Next(MAX_MAXRESTINGTICK);
        }

        public void Act()
        {
            if (_maxRestingTicks <= _restingTicks)
            {
                _mobileObject.StateEvent.FireEvent();
                return;
            }

            _restingTicks++;
        }

        public bool ShowActing
        {
            get { return false; }
        }
    }
}
