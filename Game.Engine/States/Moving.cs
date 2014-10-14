using System.Linq;

namespace Game.Engine
{
    using System;
    using Heros;
    class Moving : IState
    {
        private readonly MobileObject _mobileObject;
        private readonly Point _destination;
        private int _steps;
        private double _dx;
        private double _dy;
        private bool _isInitialized;

        public Moving(MobileObject mobileObject, Point destination)
        {
            _mobileObject = mobileObject;
            _destination = destination;
            _isInitialized = false;
        }

        private void Initialize()
        {
            double distance = Math.Sqrt((_mobileObject.Position.X - _destination.X) * (_mobileObject.Position.X - _destination.X) + (_mobileObject.Position.Y - _destination.Y) * (_mobileObject.Position.Y - _destination.Y));

            if (distance >= 0.01)
            {
                _steps = (int)(distance / _mobileObject.Speed);
                _dx = ((double)_mobileObject.Position.X - (double)_destination.X) / distance;
                _dy = ((double)_mobileObject.Position.Y - (double)_destination.Y) / distance;

                if (Math.Abs(_dx) >= 0.0001)
                    _mobileObject.Angle = (180 * Math.Atan(_dy / _dx) / Math.PI) + (_dx > 0 ? 180 : 0);
                else
                    _mobileObject.Angle = (_dy < 0) ? 90 : 270;
            }

            if (_mobileObject.PointList.Any())
                _mobileObject.PointList.RemoveAt(0);

            _isInitialized = true;
        }

        #region Implementation of IState

       // public event StateHandler NextState;

        public void Act()
        {
            if(_isInitialized == false)
                Initialize();

            _mobileObject.Position = new Point((int)(_destination.X + _dx * _steps * _mobileObject.Speed), (int)(_destination.Y + _dy * _steps * _mobileObject.Speed));
            _steps--;

            if ( /*NextState != null && */(_mobileObject.Position == _destination || _steps <= -1))
            {


                _mobileObject.StateEvent.FireEvent();
            }
            //NextState( new StateEventArgs(){State = new Standing( _hero )} );  
        }

        public bool ShowActing
        {
            get { return false; }
        }
        #endregion
    }
}
