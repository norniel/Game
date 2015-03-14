using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Actions;
using Game.Engine.Interfaces;
using Game.Engine.Objects.Fruits;
using Game.Engine.Objects.Trees;
using Game.Engine.ObjectStates;
using Game.Engine.States;
using Game.Engine.Tools;
using Microsoft.Practices.ObjectBuilder2;

namespace Game.Engine.Objects
{
    class Dikabryozik: MobileObject, IEater
    {
        private ObjectWithState ObjectWithState { get; set; }
        public Dikabryozik(Point position)
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00018000;

            Speed = 1;

            ViewRadius = 3;
            ViewSight = new Size((uint)ViewRadius, (uint)ViewRadius);
            Position = position;

            ObjectWithState = new ObjectWithState(
                new List<IObjectState>
                    {
                        new Staying() {TickCount = 300, Distribution = 30, Eternal = false},
                        new Hungry() {TickCount = 300, Distribution = 30, Eternal = true}
                    },
                    false, null, OnChangeState);

            this.StateEvent.FireEvent();
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
            };
        }

        public override string Name
        {
            get { return "Dikabryozik"; }
        }

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

            _stateQueue.Enqueue(new Wondering(this, this.ViewSight));
            return;
        }

        public override uint GetDrawingCode()
        {
            return base.GetDrawingCode() + 90+(uint)this.Angle;
        }

        public override bool CheckForUnExpected()
        {
            for (int i = 0; i < ViewSight.Width; i++)
            {
                for (int j = 0; j < ViewSight.Height; j++)
                {
                    if(i == 0 && j == 0)
                        continue;


                }
            }
            return true;
        }

        public void OnChangeState()
        {
            if (ObjectWithState != null && ObjectWithState.CurrentState is Hungry)
            {
                StartLookingForFood();
            }
        }

        public void StartLookingForFood()
        {
            _stateQueue.Clear();

            var destination = Position;

            var visiblePoints = ShadowCasting.For(PositionCell, ViewRadius, Game.Map).GetVisibleCells().OrderBy(p => p.Distance).ToList();
            //var neighbourList = Game.Map.GetNearestToPointList(Position, 1);

            var eatablePoint = visiblePoints.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj is Apple || obj is Mushroom)
                    return true;

                return false;
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
             //   var farestVisible = visiblePoints.Where(p => p.Distance == maxDistance).ToList();
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

        public void Eat()
        {
            ObjectWithState.ChangeState(0);
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
                yield return new Point(xi < 0 ? 0 : xi, yi < 0 ? 0 : yi);
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
