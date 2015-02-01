using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Actions;
using Game.Engine.Interfaces;
using Game.Engine.Objects.Fruits;
using Game.Engine.Objects.Trees;
using Game.Engine.ObjectStates;
using Game.Engine.States;

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

            ViewSight = new Size(6, 6);
            Position = position;

            ObjectWithState = new ObjectWithState(
                new List<IObjectState>
                    {
                        new Staying() {TickCount = 100, Distribution = 10, Eternal = false},
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
            var neighbourList = Game.Map.GetNearestToPointList(Position, 1);

            var eatablePoint = neighbourList.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj is Apple)
                    return true;

                return false;
            });

            if (eatablePoint != null)
            {
                _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(eatablePoint)));
                _stateQueue.Enqueue(new Eating(this, Game.Map.GetObjectFromCell(eatablePoint)));
                return;
            }
            
            var treePoint = neighbourList.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj is AppleTree && (obj as IHasSmthToCollect<Berry>).GetSmthTotalCount() > 0)
                    return true;

                return false;
            });

            if (treePoint != null)
            {
                _stateQueue.Enqueue(new Moving(this, Map.CellToPoint(treePoint)));
                _stateQueue.Enqueue(new ShakingTree(this, Game.Map.GetObjectFromCell(treePoint) as AppleTree));
                return;
            }

            while (neighbourList.Any())
            {
                var p = Game.Random.Next(neighbourList.Count);

                var obj = Game.Map.GetObjectFromCell(neighbourList[p]);
                if (obj != null && !obj.IsPassable)
                {
                    neighbourList.RemoveAt(p);
                    continue;
                }

                destination = Map.CellToPoint(neighbourList[p]);
                break;
            }

            _stateQueue.Enqueue(new Moving(this, destination));
        }

        public void Eat()
        {
            ObjectWithState.ChangeState(0);
        }
    }
}
