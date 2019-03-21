using System;
using System.Collections.Generic;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;

namespace Engine.States
{
    //this state only for hero
    class Acting : IState
    {
        private readonly MobileObject _mobileObject;
        private readonly IAction _action;
        private readonly Point _destination;
        private readonly IList<GameObject> _objects;
        private const uint MAX_TIME_STAMP = 500;
        private uint _timestamp;

        public Acting(MobileObject mobileObject, IAction action, Point destination, IList<GameObject> objects)
        {
            // TODO: Complete member initialization
            _mobileObject = mobileObject;
            _action = action;
            _destination = destination;
            _objects = objects;
            _timestamp = MAX_TIME_STAMP;
        }
        //public event StateHandler NextState;
        public void Act()
        {
            if (_timestamp < MAX_TIME_STAMP)
            {
                _timestamp += _mobileObject.Speed;
                return;
            }

            _timestamp = 0;

            bool isFinished = !IsNear(_mobileObject.Position, _destination);
            if (!isFinished)
            {
                var hero = _mobileObject as Hero;
                var actionResult = _action.Do(hero, _objects);
                isFinished = actionResult.IsFinished;
                
                actionResult.Apply(hero);
                hero?.HeroLifeCycle.IncreaseTiredness(_action.GetTiredness());
            }

            if (isFinished)
                _mobileObject.StateEvent.FireEvent();
        }

        private bool IsNear(Point position, Point destination)
        {
            return ReferenceEquals(destination, null) ||
                Math.Abs(position.X - destination.X) < 20 
                && Math.Abs(position.Y - destination.Y) < 20;
        }

        public bool ShowActing => true;
    }
}
