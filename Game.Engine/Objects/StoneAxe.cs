using System.Collections.Generic;

namespace Game.Engine.Objects
{
    class StoneAxe: FixedObject
    {
        public StoneAxe()
        {
            IsPassable = true;
            Size = new Size(1, 1);
            Id = 0x00000100;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Cutter
            };
        }

        public override string Name
        {
            get { return "Stone Axe"; }
        }
    }
}
