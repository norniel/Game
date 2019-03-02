using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.ObjectStates;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects.Animals
{
    abstract class Animal : MobileObject, IEater, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        private const int STAYING_BASE_TICKCOUNT = 300;

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

            InitProperties();

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

        protected abstract void InitProperties();

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
                _stateQueue.Enqueue(new Resting(this));
                return;
            }

            _stateQueue.Enqueue(new Wondering(this, ViewSight));
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

            var visibleCells = ShadowCasting.For(PositionCell, ViewRadius, Game.Map)
                .GetVisibleCells()
                .OrderBy(p => p.Distance)
                .ToList();

            if (LookForFood(visibleCells)) return;

            while (visibleCells.Any())
            {
                var randNumber = Game.Random.Next(visibleCells.Count);

                var obj = Game.Map.GetObjectFromCell(visibleCells[randNumber]);
                if (obj != null && !obj.IsPassable)
                {
                    visibleCells.RemoveAt(randNumber);
                    continue;
                }

                destination = visibleCells[randNumber];
                break;
            }

            EnqueueMovingToDestination(destination);
        }

        protected virtual bool LookForFood(List<PointWithDistance> visibleCells)
        {
            var eatableCell = visibleCells.FirstOrDefault(IsCellEatable);

            if (eatableCell != null)
            {
                EnqueueMovingToDestinationObject(eatableCell, Game.Map.GetObjectFromCell(eatableCell));
                _stateQueue.Enqueue(new Eating(this, () => Game.Map.GetObjectFromCell(eatableCell)));
                return true;
            }

            return false;
        }

        protected abstract bool IsCellEatable(PointWithDistance p);

        public void Eat(int satiety)
        {
            ObjectWithState.ChangeState(0, STAYING_BASE_TICKCOUNT + (int) (STAYING_BASE_TICKCOUNT * satiety * 0.1));
        }

        private IEnumerable<Point> GetMovingPointsToDestination(Point destinationCell)
        {
            var deltaX = PositionCell.X - destinationCell.X;
            var deltaY = PositionCell.Y - destinationCell.Y;
            var maxDist = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            for (int i = 1; i < maxDist; i++)
            {
                var xi = (int) Math.Round(PositionCell.X - (i * deltaX) / (double) maxDist);
                var yi = (int) Math.Round(PositionCell.Y - (i * deltaY) / (double) maxDist);

                xi = Math.Max(xi, 0);
                yi = Math.Max(yi, 0);

                var size = Game.Map.GetSize();
                xi = Math.Min(xi, (int) size.Width - 1);
                yi = Math.Min(yi, (int) size.Height - 1);

                yield return new Point(xi, yi);
            }

            yield return destinationCell;
        }

        protected void EnqueueMovingToDestination(Point destinationCell)
        {
            _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(destinationCell)));
            /*
            foreach (var p in GetMovingPointsToDestination(destinationCell))
            {
                _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(p)));
            }*/
        }

        protected void EnqueueMovingToDestinationObject(Point destinationCell, GameObject destinationObject)
        {
            _stateQueue.Enqueue(new MovingToObject(this, Map.CellToPoint(destinationCell), destinationObject));
        }
    }
}
