using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine
{
    class Tree: FixedObject, IHasBerries
    {
        private int _initialBerriesCount = 4;
        private int _berriesCount = 4;

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

        public int BerriesPerCollectCount {
            get { return 2; }
            set { }
        }
        public int BerriesCount
        {
            get { return _berriesCount; }
            set { _berriesCount = value; }
        }
        public Berry GetBerry()
        {
            return new Berry();
        }

        public override string Name
        {
            get { return "Tree"; }
        }

        public override uint GetDrawingCode()
        {
            if(this.BerriesCount > this._initialBerriesCount/2) return this.Id;

            if (this.BerriesCount <= this._initialBerriesCount / 2 && this.BerriesCount > 0) return 0x00000200;

            return 0x00000300;
        }
    }
}
