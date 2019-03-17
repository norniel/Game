using System;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.States
{
    class Eating:IState
    {
        private readonly IEater _eater;
        
        private readonly Func<GameObject> _objectToEatFunc;

        public Eating(IEater eater, Func<GameObject> objectToEatFunc)
        {
            _eater = eater;
            _objectToEatFunc = objectToEatFunc;
        }

        public void Act()
        {
            var objectToEat = _objectToEatFunc();
            var eatableBehavior = objectToEat?.GetBehavior<EatableBehavior>();
            if (eatableBehavior != null && eatableBehavior.ForType(_eater.EaterType)) {
                _eater.Eat((int)(eatableBehavior.SatietyCoefficient + objectToEat.WeightDbl));
                objectToEat.RemoveFromContainer?.Invoke();
            }

            var mobileObject = _eater as MobileObject;
            mobileObject?.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
