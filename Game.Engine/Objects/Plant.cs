using System.Collections.Generic;
using Game.Engine.Interfaces;

namespace Game.Engine.Objects
{
    internal class Plant: FixedObject, IBurnable
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
               Property.Pickable,
               Property.NeedToCreateFire
            };
        }

        public override string Name
        {
            get { return "Plant"; }
        }

        public int TimeOfBurning {
            get { return 100; }
        }
    }
}
