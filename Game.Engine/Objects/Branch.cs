using System.Collections.Generic;

namespace Game.Engine.Objects
{
    class Branch : FixedObject
    {
        public Branch() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00000800;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.NeedToCreateStoneAxe,
               Property.NeedToCreateFire
            };
        }

        public override string Name
        {
            get { return "Branch"; }
        }
    }
}
