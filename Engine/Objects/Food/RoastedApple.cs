﻿using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects.Food
{
    internal class RoastedApple : FixedObject, IEatable
    {
        public RoastedApple()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001B00;
        }

        public override int Weight => 2;

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Eatable
            };
        }

        public override string Name => Resource.RoastedApple;
        public int Poisoness => 0;

        public int Satiety => 3;
    }
}
