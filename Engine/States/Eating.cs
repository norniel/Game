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
            var eatable = _objectToEat as IEatable;
            if (eatable != null) {
                _eater.Eat(eatable.Satiety);
                _objectToEat.RemoveFromContainer();
            }

            var mobileObject = _eater as MobileObject;
            if (mobileObject != null) 
                mobileObject.StateEvent.FireEvent();
        }

        public bool ShowActing {
            get { return false; }
        }
    }
}
