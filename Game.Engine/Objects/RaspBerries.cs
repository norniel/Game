using System.Collections.Generic;

namespace Game.Engine.Objects
{
    class RaspBerries : Berry
    {
        public RaspBerries()
        {
            Id = 0x00000900;
        }

        public override string Name
        {
            get { return "Raspberries"; }
        }
    }
}
