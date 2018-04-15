using Engine.Interfaces;

namespace Engine.Behaviors
{
    class EatableBehavior : IBehavior
    {
        public int SatietyCoefficient { get; private set; }

        int Poisoness { get; }

        public EatableBehavior(int eatingCoefficient)
        {
            SatietyCoefficient = eatingCoefficient;
        }
    }
}
