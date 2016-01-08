﻿using System.Collections.Generic;
using Engine.Resources;

namespace Engine.Objects.Fruits
{
    class Cone:Berry
    {
        public Cone() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001700;
        }

        public override int Weight { get { return 2; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable
            };
        }

        public override string Name
        {
            get { return Resource.Cone; }
        }
    }
}