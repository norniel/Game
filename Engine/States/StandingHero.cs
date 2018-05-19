using Engine.Heros;

namespace Engine.States
{
    class StandingHero : StateWithBreaks, IState
    {
        private readonly Hero _hero;
        protected override uint maxTimeStamp => 20000 / Game.TimeStep;
        
        public StandingHero(Hero hero)
        {
            _hero = hero;
            timestamp = 0;
        }
        
        public void Act()
        {
            if (!TickBreak())
            {
                return;
            }

            _hero.HeroLifeCycle.DecreaseTiredness(1);
        }

        public bool ShowActing => false;
    }
}
