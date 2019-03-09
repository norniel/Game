using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using Engine.Objects.Animals;

namespace Engine.States
{
    class Killing :IState
    {
        public readonly MobileObject _hunter;
        public readonly Func<Animal> _victimFunc;
        public Killing(MobileObject hunter, Func<Animal> victimFunc)
        {
            _hunter = hunter;
            _victimFunc = victimFunc;
        }

        public void Act()
        {
            var victim = _victimFunc();
            if (victim == null || Point.Distance(_hunter.Position, victim.Position) > 1)
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
