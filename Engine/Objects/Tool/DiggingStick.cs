﻿using System.Collections.Generic;
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
        }

        public override int Weight { get { return 1; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Digger,
                Property.Pickable
            };
        }

        public override string Name
        {
            get { return Resource.DiggingStick; }
        }
    }
}