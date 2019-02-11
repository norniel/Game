using Engine.Heros;

namespace Engine.Effects
{
    interface IEffect
    {
        int Counter { get; set; }
        void Apply(HeroLifeCycle heroLifeCycle);
    }
}
