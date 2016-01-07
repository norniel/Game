using Engine.Interfaces;

namespace Engine.Objects
{
    class RaspBerries : Berry, IEatable
    {
        public RaspBerries()
        {
            Id = 0x00000900;
        }

        public override string Name
        {
            get { return "Raspberries"; }
        }

        public int Poisoness
        {
            get
            {
                return 0;
            }
        }

        public int Satiety
        {
            get
            {
                return 1;
            }
        }
    }
}
