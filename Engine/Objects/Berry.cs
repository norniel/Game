﻿using System.Collections.Generic;

namespace Engine.Objects
{
    public class Berry : FixedObject
    {
        public Berry() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00000700;
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Eatable
            };
        }

        public override string Name => "Berries";


        public override GameObject Clone()
        {
            return new Berry();
        }
    }
}
