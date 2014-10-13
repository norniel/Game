using System.Collections.Generic;

namespace Game.Engine.Objects
{
    class Dikabryozik: MobileObject
    {
        public Dikabryozik()
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00001800;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
            };
        }

        public override string Name
        {
            get { return "Dikabryozik"; }
        }
    }
}
