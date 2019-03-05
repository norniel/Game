using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects.Animals;
using Engine.ObjectStates;
using Engine.Resources;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects
{
    class Dikabryozik: Animal
    {
        private readonly Bag _bundle = new Bag(2, 2);
        
        public Dikabryozik(Point position) 
            : base(false, new Size(1,1), 0x00018000, 50, "Dikabryozik", 3, position)
        {}
        
        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
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
                EnqueueMovingToDestinationObject(eatableCell, Game.Map.GetObjectFromCell(eatableCell));
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

        protected override bool LookForFood()
        {
            if (!_bundle.IsEmpty)
            {
                _stateQueue.Enqueue(new Eating(this, () =>_bundle.GameObjects.FirstOrDefault()));
                return true;
            }

            if (base.LookForFood())
                return true;

            var treeCell = VisibleCells.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if ((obj is Tree && obj.Name == "Apple tree") &&
                    obj.GetBehavior<CollectBehavior<Berry>>()?.CurrentCount > 0)
                    return true;

                return false;
            });

            if (treeCell != null)
            {
                EnqueueMovingToDestinationObject(treeCell, Game.Map.GetObjectFromCell(treeCell));

                var appleTree = Game.Map.GetObjectFromCell(treeCell) as Tree;
                _stateQueue.Enqueue(new ShakingTree(this, () => appleTree?.GetBehavior<CollectBehavior<Berry>>()));
                return true;
            }

            return false;
        }

        protected override bool IsCellEatable(PointWithDistance p)
        {
            var obj = Game.Map.GetObjectFromCell(p);
            return (obj is Berry && obj.Name == Resource.Apple) || obj is Mushroom;
        }
    }
}
