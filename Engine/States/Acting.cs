namespace Engine.States
{
    using System;
    using System.Collections.Generic;
    using Heros;
    using Objects;
    using Wrapers;
   //this state only for hero
    class Acting : IState
    {
        private MobileObject mobileObject;
        private Interfaces.IActions.IAction action;
        private Point destination;
        private IEnumerable<GameObject> objects;
        private const uint maxTimeStamp = 500;
        private uint timestamp;

        public Acting(MobileObject mobileObject, Interfaces.IActions.IAction action, Point destination, IEnumerable<GameObject> objects)
        {
            // TODO: Complete member initialization
            this.mobileObject = mobileObject;
            this.action = action;
            this.destination = destination;
            this.objects = objects;
            timestamp = maxTimeStamp;
        }
        //public event StateHandler NextState;
        public void Act()
        {
            if (timestamp < maxTimeStamp)
            {
                timestamp += mobileObject.Speed;
                return;
            }

            timestamp = 0;

            bool isFinished = !IsNear(mobileObject.Position, destination);
            if (!isFinished)
            {
                var hero = mobileObject as Hero;
                var actionResult = action.Do(hero, objects);
                isFinished = actionResult.IsFinished;
                actionResult.Apply(hero);
                hero.HeroLifeCycle.IncreaseTiredness(action.GetTiredness());
            }

            if (isFinished)
            {
                mobileObject.StateEvent.FireEvent();
            }
        }

        private bool IsNear(Point position, Point destination)
        {
            return ReferenceEquals(destination, null) ||
                (Math.Abs(position.X - destination.X) < 20 
                && Math.Abs(position.Y - destination.Y) < 20);
        }

        public bool ShowActing => true;
    }
}
