using System.Collections.Generic;
using Engine.Resources;

namespace Engine.Objects.Tool
{
    class SharpStone:FixedObject
    {
        public SharpStone()
        {
            IsPassable = true;
            Size = new Size(1, 1);
            Id = 0x00002200;
        }

        public override int Weight => 1;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Cutter,
                Property.Pickable,
                Property.Cracker
            };
        }

        public override string Name => Resource.SharpStone;
    }
}
