using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Objects.Trees;
using Engine.ObjectStates;
using Engine.States;
using Engine.Tools;
using Microsoft.Practices.ObjectBuilder2;

namespace Engine.Objects
{
    class Dikabryozik: MobileObject, IEater
    {
        private ObjectWithState ObjectWithState { get; }

        private readonly Bag _bundle;

        private const int STAYING_BASE_TICKCOUNT = 300;

        public Dikabryozik(Point position)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00018000;

            Speed = 10;

            ViewRadius = 3;
            ViewSight = new Size((uint)ViewRadius, (uint)ViewRadius);
            Position = position;

            _bundle = new Bag(2, 2);

            ObjectWithState = new ObjectWithState(
                new List<IObjectState>
                    {
                        new Staying {TickCount = STAYING_BASE_TICKCOUNT, Distribution = STAYING_BASE_TICKCOUNT/10, Eternal = false},
                        new Hungry {TickCount = 300, Distribution = 30, Eternal = true}
                    },
                    false, null, OnChangeState);

            StateEvent.FireEvent();
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override string Name => "Dikabryozik";

        public override void EnqueueNextState()
        {
            if (ObjectWithState.CurrentState is Hungry)
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

            var visiblePoints = ShadowCasting.For(PositionCell, ViewRadius - 1, Game.Map).GetVisibleCells().OrderBy(p => p.Distance).ToList();

            var eatablePoint = visiblePoints.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj is Apple || obj is Mushroom)
                    return true;

                return false;
            });

            if (eatablePoint != null)
            {
                _stateQueue.Clear();
                EnqueueMovingToDestination(eatablePoint);
                _stateQueue.Enqueue(new Doing(this, () =>
                {
                    var obj = Game.Map.GetObjectFromCell(eatablePoint);
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
            if (ObjectWithState?.CurrentState is Hungry)
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

            var destination = Position;

            var visiblePoints = ShadowCasting.For(PositionCell, ViewRadius, Game.Map)
                .GetVisibleCells()
                .OrderBy(p => p.Distance)
                .ToList();

            //var neighbourList = Game.Map.GetNearestToPointList(Position, 1);

            var eatablePoint = visiblePoints.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                return obj is Apple || obj is Mushroom;
            });

            if (eatablePoint != null)
            {
                EnqueueMovingToDestination(eatablePoint);
                _stateQueue.Enqueue(new Eating(this, Game.Map.GetObjectFromCell(eatablePoint)));
                return;
            }

            var treePoint = visiblePoints.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj is AppleTree && (obj as IHasSmthToCollect<Berry>).GetSmthTotalCount() > 0)
                    return true;

                return false;
            });

            if (treePoint != null)
            {
                EnqueueMovingToDestination(treePoint);
                _stateQueue.Enqueue(new ShakingTree(this, Game.Map.GetObjectFromCell(treePoint) as AppleTree));
                return;
            }

            while (visiblePoints.Any())
            {
               // var maxDistance = visiblePoints.Last().Distance;
               //var farestVisible = visiblePoints.Where(p => p.Distance == maxDistance).ToList();
                var randNumber = Game.Random.Next(visiblePoints.Count);

                var obj = Game.Map.GetObjectFromCell(visiblePoints[randNumber]);
                if (obj != null && !obj.IsPassable)
                {
                    visiblePoints.RemoveAt(randNumber);
                    continue;
                }

                destination = Map.CellToPoint(visiblePoints[randNumber]);
                break;
            }

            EnqueueMovingToDestination(destination);
        }

        public void Eat(int satiety)
        {
            ObjectWithState.ChangeState(0, STAYING_BASE_TICKCOUNT + (int)(STAYING_BASE_TICKCOUNT * satiety * 0.1));
        }

        private IEnumerable<Point> GetMovingPointsToDestination(Point destinationPoint)
        {
            var deltaX = PositionCell.X - destinationPoint.X;
            var deltaY = PositionCell.Y - destinationPoint.Y;
            var maxDist = Math.Max(Math.Abs(deltaX), Math.Abs(deltaY));

            for (int i = 1; i < maxDist; i++)
            {
                var xi = (int)Math.Round((i*deltaX)/(double)maxDist + PositionCell.X);
                var yi = (int)Math.Round((i * deltaY) / (double)maxDist + PositionCell.Y);

                xi = Math.Max(xi, 0);
                yi = Math.Max(yi, 0);

                var size = Game.Map.GetSize();
                xi = Math.Min(xi, (int)size.Width);
                yi = Math.Min(yi, (int)size.Height);
                
                yield return new Point(xi, yi);
            }

            yield return destinationPoint;
        }

        private void EnqueueMovingToDestination(Point destinationPoint)
        {
            GetMovingPointsToDestination(destinationPoint).ForEach(p =>
            {
                _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(p)));
            });
        }
    }
}
