using Engine.Interfaces;

namespace Engine.Behaviors
{
    public class EatableBehavior : IBehavior
    {
        public int SatietyCoefficient { get; set; }

        public int Poisoness { get; set; }
        public int Time { get; set; }
        internal EaterType EaterType { get; set; } = EaterType.Human;
        
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

        public bool ForHuman()
        {
            return EaterType.HasFlag(EaterType.Human);
        }

        internal bool ForType(Engine.EaterType eaterType)
        {
            return EaterType.HasFlag(eaterType);
        }
    }
}
