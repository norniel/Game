using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Objects.Trees;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects
{
    class Dikabryozik: MobileObject, IEater, IWithObjectWithState
    {
        public ObjectWithState ObjectWithState { get; }

        private readonly Bag _bundle;

        private const int STAYING_BASE_TICKCOUNT = 300;

        public Dikabryozik(Point position)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00018000;

            Speed = 50;

            Name = "Dikabryozik";

            ViewRadius = 3;
            ViewSight = new Size((uint)ViewRadius, (uint)ViewRadius);
            Position = position;

            _bundle = new Bag(2, 2);

            ObjectWithState = new ObjectWithState(
                new List<ObjectState>
                    {
                        new ObjectState(ObjectStates.ObjectStates.Staying) {TickCount = STAYING_BASE_TICKCOUNT, Distribution = STAYING_BASE_TICKCOUNT/10, Eternal = false},
                        new ObjectState(ObjectStates.ObjectStates.Hungry){TickCount = 300, Distribution = 30, Eternal = true}
                    },
                    false, null, OnChangeState);

            StateEvent.FireEvent();
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override void EnqueueNextState()
        {
            if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Hungry)
            {
                StartLookingForFood();
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

        public override uint GetDrawingCode()
        {
            if(_bundle.IsEmpty)
                return base.GetDrawingCode() + 90+(uint)Angle;

            return 0x10018000 + 90 + (uint)Angle;
        }

        public override bool CheckForUnExpected()
        {
            if (_bundle.IsFull || !(State is Resting || State is Wondering))
                return true;

            var visibleCells = ShadowCasting.For(PositionCell, ViewRadius - 1, Game.Map).GetVisibleCells().OrderBy(p => p.Distance).ToList();

            var eatableCell = visibleCells.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if ((obj is Berry && obj.Name == Resource.Apple) || obj is Mushroom)
                    return true;

                return false;
            });

            if (eatableCell != null)
            {
                _stateQueue.Clear();
                EnqueueMovingToDestination(eatableCell);
                _stateQueue.Enqueue(new Doing(this, () =>
                {
                    var obj = Game.Map.GetObjectFromCell(eatableCell);
                    if(obj != null)
                        _bundle.Add(obj);

                }));

                StateEvent.FireEvent();
                return false;
            }

            return true;
        }

        private void OnChangeState()
        {
            if (ObjectWithState?.CurrentState.Name == ObjectStates.ObjectStates.Hungry)
            {
                StartLookingForFood();
            }
        }

        private void StartLookingForFood()
        {
            _stateQueue.Clear();

            if (!_bundle.IsEmpty)
            {
                _stateQueue.Enqueue(new Eating(this, _bundle.GameObjects.First()));
                return;
            }

            var destination = PositionCell;

            var visibleCells = ShadowCasting.For(PositionCell, ViewRadius, Game.Map)
                .GetVisibleCells()
                .OrderBy(p => p.Distance)
                .ToList();

            //var neighbourList = Game.Map.GetNearestToPointList(Position, 1);

            var eatableCell = visibleCells.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                return (obj is Berry && obj.Name == Resource.Apple) || obj is Mushroom;
            });

            if (eatableCell != null)
            {
                EnqueueMovingToDestination(eatableCell);
                _stateQueue.Enqueue(new Eating(this, Game.Map.GetObjectFromCell(eatableCell)));
                return;
            }

            var treeCell = visibleCells.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if ((obj is Tree && obj.Name == "Apple tree") && (obj.GetBehavior(typeof(CollectBehavior<Berry>)) as CollectBehavior<Berry>)?.CurrentCount > 0)
                    return true;

                return false;
            });

            if (treeCell != null)
            {
                EnqueueMovingToDestination(treeCell);
                var appleTree = Game.Map.GetObjectFromCell(treeCell) as Tree;
                _stateQueue.Enqueue(new ShakingTree(this, appleTree?.GetBehavior(typeof(CollectBehavior<Berry>)) as CollectBehavior<Berry>));
                return;
            }

            while (visibleCells.Any())
            {
               // var maxDistance = visiblePoints.Last().Distance;
               //var farestVisible = visiblePoints.Where(p => p.Distance == maxDistance).ToList();
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

        public void Eat(int satiety)
        {
            ObjectWithState.ChangeState(0, STAYING_BASE_TICKCOUNT + (int)(STAYING_BASE_TICKCOUNT * satiety * 0.1));
        }

        private IEnumerable<Point> GetMovingPointsToDestination(Point destinationCell)
        {
            var deltaX = PositionCell.X - destinationCell.X;
            var deltaY = PositionCell.Y - destinationCell.Y;
            var maxDist = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            for (int i = 1; i < maxDist; i++)
            {
                var xi = (int)Math.Round(PositionCell.X - (i*deltaX)/(double)maxDist);
                var yi = (int)Math.Round(PositionCell.Y - (i * deltaY) / (double)maxDist);

                xi = Math.Max(xi, 0);
                yi = Math.Max(yi, 0);

                var size = Game.Map.GetSize();
                xi = Math.Min(xi, (int)size.Width - 1);
                yi = Math.Min(yi, (int)size.Height - 1);
                
                yield return new Point(xi, yi);
            }

            yield return destinationCell;
        }

        private void EnqueueMovingToDestination(Point destinationPoint)
        {
            foreach(var p in GetMovingPointsToDestination(destinationPoint))
            {
                _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(p)));
            };
        }
    }
}
