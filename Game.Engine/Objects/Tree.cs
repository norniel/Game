using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine
{
    class Tree: FixedObject, IHasBerries
    {
        public Tree()
        {
            IsPassable = false;

            Size = new Size( 1, 1 );

            Id = 0x00000100;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Cuttable,
                Property.CollectBerries
            };
        }

        public int BerriesPerCollectCount { get; set; }
        public int BerriesCount { get; set; }
        public Berry GetBerry()
        {
            return new Berry();
        }

        public override string Name
        {
            get { return "Tree"; }
        }
    }
}
