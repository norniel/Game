using Engine.Heros;
namespace Engine.States
{
    class Halting : StateWithBreaks, IState
    {
        protected override uint maxTimeStamp => 50;
        
        private readonly Hero _hero;
        public Halting(Hero hero)
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

            _hero.StateEvent.FireEvent();
        }

        public bool ShowActing => false;
    }
}
