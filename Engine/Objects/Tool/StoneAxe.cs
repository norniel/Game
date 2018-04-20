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
            Name = Resource.StoneAxe;
        }

        public override int Weight => 10;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Cutter,
                Property.Pickable,
                Property.Cracker
            };
        }
    }
}
