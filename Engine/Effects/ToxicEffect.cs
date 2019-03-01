using Engine.Heros;

namespace Engine.Effects
{
    class ToxicEffect:IEffect
    {
        public int Counter { get; set; }
        private readonly int _poisonness;

        public ToxicEffect(int poisonness, int counter)
        {
            _poisonness = poisonness;
            Counter = counter;
        }

        public void Apply(HeroLifeCycle heroLifeCycle)
        {
            heroLifeCycle.ApplyToxic(_poisonness);
        }
    }
}
