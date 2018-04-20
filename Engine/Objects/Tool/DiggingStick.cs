using System.Collections.Generic;
using Engine.Resources;

namespace Engine.Objects.Tool
{
    class DiggingStick: FixedObject
    {
        public DiggingStick()
        {
            IsPassable = true;
            Size = new Size(1, 1);
            Id = 0x00002300;
            Name = Resource.DiggingStick;
        }

        public override int Weight => 1;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Digger,
                Property.Pickable
            };
        }
    }
}
