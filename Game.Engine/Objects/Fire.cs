using System.Collections.Generic;

namespace Game.Engine.Objects
{
    internal class Fire: FixedObject 
    {
        public Fire() 
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00000600;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {};
        }

        public override string Name
        {
            get { return "Fire"; }
        }
    }
}
