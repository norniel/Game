using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine.States
{
    class Eating:IState
    {
        private readonly IEater _eater;
        private GameObject _objectToEat;

        public Eating(IEater eater, GameObject objectToEat)
        {
            _eater = eater;
            _objectToEat = objectToEat;
        }
        public void Act()
        {
            _eater.Eat();
            _objectToEat.RemoveFromContainer();
            (_eater as MobileObject).StateEvent.FireEvent();
        }

        public bool ShowActing {
            get { return false; }
        }
    }
}
