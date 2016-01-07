using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Objects
{
    class Twig : FixedObject, IBurnable
    {
        public Twig() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001C00;
        }

        public override int Weight { get { return 1; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable
            };
        }

        public int TimeOfBurning
        {
            get { return 100; }
        }

        public override string Name
        {
            get { return "Twig"; }
        }
    }
}
