using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    internal class RoastedMushroom : FixedObject, IEatable
    {
        public RoastedMushroom()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001A00;
        }

        public override int Weight => 2;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.Eatable
            };
        }

        public override string Name => Resource.RoastedBurovik;

        public int Poisoness => 0;

        public int Satiety => 5;
    }
}
