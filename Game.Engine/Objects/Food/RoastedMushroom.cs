using System.Collections.Generic;
using Game.Engine.Interfaces;

namespace Game.Engine.Objects
{
    internal class RoastedMushroom : FixedObject, IEatable
    {
        public RoastedMushroom()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001A00;
        }


        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Eatable
            };
        }

        public override string Name
        {
            get { return "Roasted burovik"; }
        }

        public int Poisoness
        {
            get { return 0; }
        }

        public int Satiety
        {
            get { return 5; }
        }
    }
}
