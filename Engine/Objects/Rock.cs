using System.Collections.Generic;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    [GenerateMap]
    class Rock : FixedObject
    {
        public Rock() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001000;
            Name = Resource.Rock;
        }

        public int Fragile { get; set; }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Pickable,
                Property.NeedToCreateStoneAxe,
                Property.Stone,
                Property.Cracker
            };
        }
    }
}
