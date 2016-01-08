using System.Collections.Generic;
using Engine.Resources;

namespace Engine.Objects
{
    class StoneAxe: FixedObject
    {
        public StoneAxe()
        {
            IsPassable = true;
            Size = new Size(1, 1);
            Id = 0x00001300;
        }

        public override int Weight { get { return 10; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Cutter,
                Property.Pickable
            };
        }

        public override string Name
        {
            get { return Resource.StoneAxe; }
        }
    }
}
