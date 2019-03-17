using System;
using Engine.Objects.Animals;

namespace Engine.States
{
    class Killing :IState
    {
        private readonly MobileObject _hunter;
        private readonly Func<Animal> _victimFunc;
        public Killing(MobileObject hunter, Func<Animal> victimFunc)
        {
            _hunter = hunter;
            _victimFunc = victimFunc;
        }

        public void Act()
        {
            var victim = _victimFunc();
            if (victim == null || victim.IsDead || Point.Distance(_hunter.Position, victim.Position) > 1)
            {
                _hunter.StateEvent.FireEvent();
                return;
            }

            victim.Die();
            _hunter.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
