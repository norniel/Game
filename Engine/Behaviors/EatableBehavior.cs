using Engine.Interfaces;

namespace Engine.Behaviors
{
    public class EatableBehavior : IBehavior
    {
        public int SatietyCoefficient { get; set; }

        public int Poisoness { get; set; } = 0;
        public int Time { get; set; } = 0;

        public EatableBehavior(int eatingCoefficient)
        {
            SatietyCoefficient = eatingCoefficient;
        }

        public EatableBehavior(int eatingCoefficient, int poisoness, int time)
        {
            SatietyCoefficient = eatingCoefficient;
            Poisoness = poisoness;
            Time = time;
        }
    }
}
