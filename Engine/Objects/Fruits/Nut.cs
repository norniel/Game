using System.Collections.Generic;

namespace Engine.Objects.Fruits
{
    class Nut: Berry
    {
        public Nut()
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00002600;
        }

        public override int Weight => 1;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Pickable,
                Property.Crackable
            };
        }

        public override string Name => "Nut";

        public override GameObject Clone()
        {
            return new Nut();
        }
    }
}