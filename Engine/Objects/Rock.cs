using System;
using System.Collections.Generic;

namespace Engine.Objects
{
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
               Property.NeedToCreateStoneAxe
            };
        }

        public override string Name
        {
            get { return "Rock"; }
        }
    }
}
