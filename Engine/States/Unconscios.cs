using Engine.Heros;

namespace Engine.States
{
    class Unconscios:StateWithBreaks, IState
    {
        private readonly Hero _hero;
        public Unconscios(Hero hero)
        {
            _hero = hero;
        }
        public void Act()
        {
            if (!TickBreak())
            {
                return;
            }

            bool isOver = _hero.HeroLifeCycle.HeroProperties.Tiredness <= 80;
            if (!isOver)
            {
                _hero.HeroLifeCycle.DecreaseTiredness(20);
                isOver = _hero.HeroLifeCycle.HeroProperties.Tiredness <= 80;
            }

            if(isOver)
                _hero.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
