using System.ComponentModel;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;
using JetBrains.Annotations;

namespace Engine.States
{
    class Eating:IState
    {
        private readonly IEater _eater;

        [CanBeNull]
        private readonly GameObject _objectToEat;

        public Eating(IEater eater, [CanBeNull] GameObject objectToEat)
        {
            _eater = eater;
            _objectToEat = objectToEat;
        }
        public void Act()
        {
            var eatableBehavior = _objectToEat?.GetBehavior(typeof(EatableBehavior)) as EatableBehavior;
            if (eatableBehavior != null) {
                _eater.Eat((int)(eatableBehavior.SatietyCoefficient + _objectToEat.WeightDbl));
                _objectToEat.RemoveFromContainer?.Invoke();
            }

            var mobileObject = _eater as MobileObject;
            mobileObject?.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
