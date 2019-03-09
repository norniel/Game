using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Objects.Animals;

namespace Engine.States
{
    class Pursuing: IState
    {
        private readonly MobileObject _mobileObject;
        private readonly Animal _destinationObject;
        private int _pursuingInvisible;

        public Pursuing(MobileObject mobileObject, Animal destinationObject)
        {
            _mobileObject = mobileObject;
            _destinationObject = destinationObject;
        }

        public void Act()
        {
            if (_destinationObject.IsDead)
            {
                _mobileObject.StateEvent.FireEvent();
                return;
            }

            var destination = _destinationObject.Position;

            if (_mobileObject.VisibleCells?.All(vc => vc != _destinationObject.PositionCell) ?? true)
            {
                if (_pursuingInvisible < 5)
                {
                    _pursuingInvisible++;
                }
                else
                {
                    _mobileObject.StateEvent.FireEvent();
                    return;
                }
            }

            double distance = Math.Sqrt((_mobileObject.Position.X - destination.X) * (_mobileObject.Position.X - destination.X) + (_mobileObject.Position.Y - destination.Y) * (_mobileObject.Position.Y - destination.Y));

            if (distance >= 0.01)
            {
                var dx = (_mobileObject.Position.X - (double)destination.X) / distance;
                var dy = (_mobileObject.Position.Y - (double)destination.Y) / distance;

                if (Math.Abs(dx) >= 0.0001)
                    _mobileObject.Angle = (180 * Math.Atan(dy / dx) / Math.PI) + (dx > 0 ? 180 : 0);
                else
                    _mobileObject.Angle = (dy < 0) ? 90 : 270;

                _mobileObject.Position = new Point((int)(_mobileObject.Position.X - dx *  _mobileObject.Speed / 10), (int)(_mobileObject.Position.Y - dy * _mobileObject.Speed / 10));
            }
            else
            {
                _mobileObject.StateEvent.FireEvent();
            }
        }

        public bool ShowActing => false;
    }
}
