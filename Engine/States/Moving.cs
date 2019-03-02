using System;
using System.Linq;
using Engine.Heros;

namespace Engine
{
    class Moving : IState
    {
        protected readonly MobileObject _mobileObject;
        protected Point _destination;
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

        protected virtual void Initialize()
        {
            double distance = Math.Sqrt((_mobileObject.Position.X - _destination.X) * (_mobileObject.Position.X - _destination.X) + (_mobileObject.Position.Y - _destination.Y) * (_mobileObject.Position.Y - _destination.Y));

            if (distance >= 0.01)
            {
                _steps = (int)(distance*10/ _mobileObject.Speed);
                _dx = (_mobileObject.Position.X - (double)_destination.X) / distance;
                _dy = (_mobileObject.Position.Y - (double)_destination.Y) / distance;

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
        
        public virtual void Act()
        {
            if(_isInitialized == false)
                Initialize();

            _mobileObject.Position = new Point((int)(_destination.X + _dx * _steps * _mobileObject.Speed/10), (int)(_destination.Y + _dy * _steps * _mobileObject.Speed/10));

            HeroAdditional();

            _steps--;

            if (_mobileObject.Position == _destination || _steps <= -1)
            {
                if (_mobileObject is Hero hero)
                {
                    hero.HeroLifeCycle.IncreaseTiredness(0.1);
                }

                _mobileObject.StateEvent.FireEvent();
            }
        }

        private void HeroAdditional()
        {
            if (_mobileObject is Hero)
            {
                var cell = Map.PointToCell(_mobileObject.Position);
                if (!Game.Map.CellInInnerMap(cell))
                {
                    Game.Map.ClearInnerMap();
                }
            }
        }

        public bool ShowActing => false;

        #endregion
    }
}
