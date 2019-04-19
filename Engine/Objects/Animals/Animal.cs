using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.States;
using Engine.Tools;
using Unity;

namespace Engine.Objects.Animals
{
    abstract class Animal : MobileObject, IEater, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        [Dependency]
        public Hero Hero { get; set; }

        protected const int STAYING_BASE_TICKCOUNT = 300;

        public bool IsDead { get; protected set; }
        
        protected Animal(bool isPassable, Size size, uint id, uint speed, string name, int viewRadius, Point position)
        {
            IsPassable = isPassable;

            Size = size;

            Id = id;

            Speed = speed;

            Name = name;

            ViewRadius = viewRadius;
            ViewSight = new Size((uint)ViewRadius, (uint)ViewRadius);
            Position = position;

            ObjectWithState = new ObjectWithState(
                new List<ObjectState>
                {
                    new ObjectState(ObjectStates.ObjectStates.Staying)
                    {
                        TickCount = STAYING_BASE_TICKCOUNT, Distribution = STAYING_BASE_TICKCOUNT / 10, Eternal = false
                    },
                    new ObjectState(ObjectStates.ObjectStates.Hungry)
                        {TickCount = 300, Distribution = 30, Eternal = true}
                },
                false, null, OnChangeState);

            StateEvent.FireEvent();
        }

        public override void EnqueueNextState()
        {
            if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Hungry)
            {
                LookForFoodOrMove();
                return;
            }

            var dice = Game.Random.Next(3);
            if (dice == 2)
            {
                _stateQueue.Enqueue(new Wondering(this, ViewSight));
                return;
            }

            _stateQueue.Enqueue(new Resting(this));
        }

        private void OnChangeState()
        {
            if (ObjectWithState?.CurrentState.Name == ObjectStates.ObjectStates.Hungry)
            {
                LookForFoodOrMove();
            }
        }

        private void LookForFoodOrMove()
        {
            _stateQueue.Clear();

            var destination = PositionCell;

            if (LookForFood()) return;
            var passable = VisibleCells
                .Where(x => Game.Map.GetObjectFromCell(x)?.IsPassable ?? true)
                .ToList();

            if (passable.Any())
                destination = passable[Game.Random.Next(passable.Count)];

            EnqueueMovingToDestination(destination);
        }

        protected virtual bool LookForFood()
        {
            var eatableCell = VisibleCells.FirstOrDefault(IsCellEatable);

            if (eatableCell != null)
            {
                EnqueueMovingToDestinationObject(eatableCell, Game.Map.GetObjectFromCell(eatableCell));
                _stateQueue.Enqueue(new Eating(this, () => Game.Map.GetObjectFromCell(eatableCell)));

                return true;
            }

            return false;
        }

        protected abstract bool IsCellEatable(PointWithDistance p);

        public virtual void Eat(int satiety)
        {
            ObjectWithState.ChangeState(0, STAYING_BASE_TICKCOUNT + (int) (STAYING_BASE_TICKCOUNT * satiety * 0.1));
        }

        public abstract EaterType EaterType { get; }

        private IEnumerable<Point> GetMovingPointsToDestination(Point destinationCell)
        {
            var deltaX = PositionCell.X - destinationCell.X;
            var deltaY = PositionCell.Y - destinationCell.Y;
            var maxDist = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            for (int i = 1; i < maxDist; i++)
            {
                var xi = (int) Math.Round(PositionCell.X - i * deltaX / (double) maxDist);
                var yi = (int) Math.Round(PositionCell.Y - i * deltaY / (double) maxDist);

                xi = Math.Max(xi, 0);
                yi = Math.Max(yi, 0);

                var size = Game.Map.GetSize();
                xi = Math.Min(xi, (int) size.Width - 1);
                yi = Math.Min(yi, (int) size.Height - 1);

                yield return new Point(xi, yi);
            }

            yield return destinationCell;
        }

        private void EnqueueMovingToDestination(Point destinationCell)
        {
            _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(destinationCell)));
        }

        protected void EnqueueMovingToDestinationObject(Point destinationCell, GameObject destinationObject)
        {
            _stateQueue.Enqueue(new MovingToObject(this, Map.CellToPoint(destinationCell), destinationObject));
        }

        public virtual IEnumerable<MobileObject> GetEnemies()
        {
            return null;
        }

        public virtual void Die()
        {
            IsDead = true;
        }

        protected bool FleeEnemies()
        {
            var enemiesExist = GetEnemies()?.Any() ?? false;

            if (!enemiesExist)
                return false;

            _stateQueue.Clear();
            _stateQueue.Enqueue(new Fleeing(this));
            StateEvent.FireEvent();

            return true;
        }
        

        public override bool CheckForUnExpected()
        {
            return !FleeEnemies();
        }
    }
}
