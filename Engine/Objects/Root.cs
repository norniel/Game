﻿using System.Collections.Generic;
using Engine.Resources;

namespace Engine.Objects
{
    class Root:FixedObject
    {
        public Root()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00002400;
        }

        public override int Weight { get { return 1; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Diggable
            };
        }

        public override string Name
        {
            get { return Resource.Root; }
        }
    }
}