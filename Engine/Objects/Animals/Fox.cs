using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.States;
using Engine.Tools;

namespace Engine.Objects.Animals
{
    class Fox : Animal
    {
        public Fox(Point position)
            : base(false, new Size(1, 1), 0x00020000, 50, "Fox", 4, position)
        {
        }

        protected override bool IsCellEatable(PointWithDistance p)
        {
            throw new NotImplementedException();
        }

        public override EaterType EaterType => EaterType.Carnivorous;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>();
        }

        public override uint GetDrawingCode()
        {
            return 0x00020000 + 90 + (uint) Angle;
        }

        public override void Eat(int satiety)
        {
            if (ObjectWithState.CurrentState.Name == ObjectStates.ObjectStates.Hungry)
                ObjectWithState.ChangeState(0, STAYING_BASE_TICKCOUNT + (int) (STAYING_BASE_TICKCOUNT * satiety * 0.1));
            else
            {
                ObjectWithState.ChangeState(0,
                    ObjectWithState.TicksToNextState + (int) (STAYING_BASE_TICKCOUNT * satiety * 0.1));
            }
        }

        public override bool CheckForUnExpected()
        {
            if (State is Fleeing)
                return true;

            if (FleeEnemies())
                return false;

            if (State is Pursuing || State is Killing || State is MovingToObject || State is Eating)
                return true;
            
            if (!GoToFoodAndEat())
                return true;

            StateEvent.FireEvent();
            return false;
        }

        private bool GoToFoodAndEat()
        {
            var objWithMeatCell = VisibleCells.FirstOrDefault(p =>
            {
                var obj = Game.Map.GetObjectFromCell(p);
                if (obj != null && obj.HasBehavior<CollectBehavior<Meat>>() &&
                    obj.GetBehavior<CollectBehavior<Meat>>()?.CurrentCount > 0)
                    return true;

                return false;
            });

            if (objWithMeatCell == null)
                return false;

            _stateQueue.Clear();

            EnqueueMovingToDestinationObject(objWithMeatCell, Game.Map.GetObjectFromCell(objWithMeatCell));

            GameObject GetObjFunc()
            {
                var objWithMeat = Game.Map.GetObjectFromCell(objWithMeatCell);
                if (!objWithMeat.HasBehavior<CollectBehavior<Meat>>() ||
                    objWithMeat.GetBehavior<CollectBehavior<Meat>>()?.CurrentCount <= 0)
                    return null;

                var meatBehavior = objWithMeat.GetBehavior<CollectBehavior<Meat>>();
                meatBehavior.CurrentCount = meatBehavior.CurrentCount - 1;
                return meatBehavior.GetSmth();
            }

            _stateQueue.Enqueue(new Eating(this, GetObjFunc));
            return true;
        }

        protected override bool LookForFood()
        {
            if (GoToFoodAndEat())
                return true;

            var destinationObject = VisibleCells.Select(vCell =>
            {
                var t = Game.Map.GetMobileObject(vCell);
                return t;
            }).OfType<Animal>().FirstOrDefault(mo => { return mo != null && mo.Name == "Hare" && !mo.IsDead; });

            if (destinationObject != null)
            {
                _stateQueue.Enqueue(new Pursuing(this, destinationObject as Animal));
                _stateQueue.Enqueue(new Killing(this, () => destinationObject as Animal));

                return true;
            }

            return false;
        }

        public override IEnumerable<MobileObject> GetEnemies()
        {
            return VisibleCells.Where(vc => Hero.PositionCell == vc).Select(vc => Hero);
        }

        public override bool IsMoving => State is Moving || State is Fleeing || State is Pursuing || State is MovingToObject;
    }
}
