using System.Collections.Generic;

namespace Game.Engine.Objects.LargeObjects
{
    class GrassBed:LargeObjectInner
    {
        public GrassBed()
        {
            Size = new Size(1, 2);
            IsPassable = false;
            Id = 0x00001D00;
        }

        public static int CountToBuild = 8; 

        public override string Name
        {
            get { return "Grass bed"; }
        }

        public override uint GetDrawingCode()
        {
            if(IsBuild)
                return this.Id;

            var buildProcents = CountLeftToBuild * 100 / CountToBuild;
            if (buildProcents <= 30) {
                return 0x00001D01;
            }
            else if (buildProcents <= 60)
            {
                return 0x00001D02;
            }

            return 0x00001D03;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.NeedToBuildGrassBed,
               Property.NeedToSleep
            };
        }

        private int _countLeftToBuild = CountToBuild;

        public int CountLeftToBuild { 
            get { return _countLeftToBuild; } 
            set { _countLeftToBuild = value > 0 ? value : 0; } 
        }

        public bool IsBuild { get { return CountLeftToBuild <= 0; } }
    }
}
