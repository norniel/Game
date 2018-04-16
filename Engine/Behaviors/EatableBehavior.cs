using Engine.Interfaces;

namespace Engine.Behaviors
{
    public class EatableBehavior : IBehavior
    {
        public int SatietyCoefficient { get; set; }

        int Poisoness { get; }

        public EatableBehavior(int eatingCoefficient)
        {
            SatietyCoefficient = eatingCoefficient;
        }
    }
}
