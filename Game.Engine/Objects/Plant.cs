using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Engine.Interfaces;

namespace Game.Engine.Objects
{
    internal class Plant: FixedObject
    {
        public Plant() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001100;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable
            };
        }

        public override string Name
        {
            get { return "Plant"; }
        }
    }
}
