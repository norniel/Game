using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Resources;

namespace Engine.Objects
{
   /* internal class RoastedMushroom : FixedObject
    {
        public RoastedMushroom()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00001A00;
            Name = Resource.RoastedBurovik;
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

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new EatableBehavior(5));
        }
    }*/
}
