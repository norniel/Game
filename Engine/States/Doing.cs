using Engine.Interfaces;
using System;

namespace Engine.States
{
    class Doing:IState
    {
        private readonly Action _action;
        private readonly MobileObject _mobileObject;
        public Doing(MobileObject mobileObject, Action action)
        {
            _action = action;
            _mobileObject = mobileObject;
        }
        public void Act()
        {
            _action();
            _mobileObject.StateEvent.FireEvent();
        }

        public bool ShowActing {
            get { return false; }
        }
    }
}
