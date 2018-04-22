using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.States
{
    class Eating:IState
    {
        private readonly IEater _eater;
        private readonly GameObject _objectToEat;

        public Eating(IEater eater, GameObject objectToEat)
        {
            _eater = eater;
            _objectToEat = objectToEat;
        }
        public void Act()
        {
            var eatableBehavior = _objectToEat.GetBehavior(typeof(EatableBehavior)) as EatableBehavior;
            if (eatableBehavior != null) {
                _eater.Eat((int)(eatableBehavior.SatietyCoefficient + _objectToEat.WeightDbl), eatableBehavior.Poisoness, eatableBehavior.Time);
                _objectToEat.RemoveFromContainer?.Invoke();
            }

            var mobileObject = _eater as MobileObject;
            if (mobileObject != null) 
                mobileObject.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
