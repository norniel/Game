using Engine.Interfaces;

namespace Engine.Objects
{
    class RaspBerries : Berry, IEatable
    {
        public RaspBerries()
        {
            Id = 0x00000900;
        }

        public override string Name => "Raspberries";

        public int Poisoness => 0;

        public int Satiety => 1;
    }
}
