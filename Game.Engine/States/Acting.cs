namespace Game.Engine.States
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
        private const uint maxTimeStamp = 50;
        private uint timestamp;

        public Acting(MobileObject mobileObject, Interfaces.IActions.IAction action, Point destination, IEnumerable<GameObject> objects)
        {
            // TODO: Complete member initialization
            this.mobileObject = mobileObject;
            this.action = action;
            this.destination = destination;
            this.objects = objects;
            this.timestamp = maxTimeStamp;
        }
        //public event StateHandler NextState;
        public void Act()
        {
            if (this.timestamp < maxTimeStamp)
            {
                this.timestamp += mobileObject.Speed;
                return;
            }

            this.timestamp = 0;

            bool isFinished = !this.IsNear(mobileObject.Position, destination) || action.Do(mobileObject as Hero, objects);

            if(isFinished)
                mobileObject.StateEvent.FireEvent();
        }

        private bool IsNear(Point position, Point destination)
        {
            return ReferenceEquals(destination, null) ||
                (Math.Abs(position.X - destination.X) < 20 
                && Math.Abs(position.Y - destination.Y) < 20);
        }

        public bool ShowActing
        {
            get { return true; }
        }

    }
}
