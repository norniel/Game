using System;
using System.Collections.Generic;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.States
{
    class Acting : IState
    {
        private Hero hero;
        private Interfaces.IActions.IAction action;
        private Point destination;
        private IEnumerable<RemovableWrapper<GameObject>> objects;
        private const uint maxTimeStamp = 50;
        private uint timestamp;

        public Acting(Hero hero, Interfaces.IActions.IAction action, Point destination, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            // TODO: Complete member initialization
            this.hero = hero;
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
                this.timestamp+= hero.Speed;
                return;
            }

            this.timestamp = 0;

            bool isFinished = !this.IsNear(hero.Position, destination) || action.Do(hero, objects);

            if(isFinished)
                StateEvent.FireEvent();
        }

        private bool IsNear(Point position, Point destination)
        {
            return Math.Abs(position.X - destination.X) < 20 
                && Math.Abs(position.Y - destination.Y) < 20;
        }
    }
}
