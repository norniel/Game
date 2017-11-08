using Engine.Heros;

namespace Engine.States
{
    class Sleeping:StateWithBreaks, IState
    {
        private readonly Hero _hero;
        public Sleeping(Hero hero)
        {
            _hero = hero;
            this.timestamp = 0;
        }
        public void Act()
        {
            if (!this.TickBreak())
            {
                return;
            }

            bool isOver = _hero.HeroLifeCycle.HeroProperties.Tiredness <= 0;
            if (!isOver)
            {
                _hero.HeroLifeCycle.DecreaseTiredness(20);
                isOver = _hero.HeroLifeCycle.HeroProperties.Tiredness <= 0;
            }

            if(isOver)
                _hero.StateEvent.FireEvent();
        }

        public bool ShowActing => true;
    }
}
