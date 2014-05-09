namespace Game.Engine
{
    using System;
    using Heros;
    class Moving : IState
    {
        private readonly Hero _hero;
        private readonly Point _destination;
        private int _steps;
        private double _dx;
        private double _dy;
        private bool _isInitialized;

        public Moving( Hero hero, Point destination )
        {
            _hero = hero;
            _destination = destination;
            _isInitialized = false;
        }

        private void Initialize()
        {
            double distance = Math.Sqrt((_hero.Position.X - _destination.X) * (_hero.Position.X - _destination.X) + (_hero.Position.Y - _destination.Y) * (_hero.Position.Y - _destination.Y));

            if (distance >= 0.01)
            {
                _steps = (int)(distance / _hero.Speed);
                _dx = ((double)_hero.Position.X - (double)_destination.X) / distance;
                _dy = ((double)_hero.Position.Y - (double)_destination.Y) / distance;

                if (Math.Abs(_dx) >= 0.0001)
                    _hero.Angle = (180 * Math.Atan(_dy / _dx) / Math.PI) + (_dx > 0 ? 180 : 0);
                else
                    _hero.Angle = (_dy < 0) ? 90 : 270;
            }

            _isInitialized = true;
        }

        #region Implementation of IState

       // public event StateHandler NextState;

        public void Act()
        {
            if(_isInitialized == false)
                Initialize();

            _hero.Position = new Point( (int) (_destination.X + _dx * _steps * _hero.Speed), (int)(_destination.Y + _dy * _steps * _hero.Speed));
            _steps--;

            if( /*NextState != null && */( _hero.Position == _destination || _steps <= -1 ))
                //NextState( new StateEventArgs(){State = new Standing( _hero )} );
                StateEvent.FireEvent();
        }

        #endregion
    }
}
