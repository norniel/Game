using System.Collections.Generic;

namespace Game.Engine.Objects
{
    class Berry : FixedObject
    {
        public Berry() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00000700;
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
            get { return "Berries"; }
        }

        public int Poisoness { get; set; }
        public int Satiety { get; set; }
    }
}
