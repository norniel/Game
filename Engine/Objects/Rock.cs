using System;
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
        }

        public int Fragile { get; set; }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Pickable,
                Property.NeedToCreateStoneAxe,
                Property.Stone,
                Property.Cracker
            };
        }

        public override string Name => Resource.Rock;
    }
}
