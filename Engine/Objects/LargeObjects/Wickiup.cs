using System.Collections.Generic;
using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.Builder;
using Engine.Resources;

namespace Engine.Objects
{
    class Wickiup:LargeObjectInner
    {
        public Wickiup()
        {
            Size = new Size(2, 3);
            IsPassable = false;
            Id = 0x00001E00;
        }

        public static int CountToBuild = 8; 

        public override string Name
        {
            get { return Resource.Wickiup; }
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
            };
        }

        private int _countLeftToBuild = CountToBuild;

        public int CountLeftToBuild { 
            get { return _countLeftToBuild; } 
            set { _countLeftToBuild = value > 0 ? value : 0; } 
        }

        public bool IsBuild { get { return CountLeftToBuild <= 0; } }
        protected override BuilderPlan GetBuilderPlan()
        {
            throw new System.NotImplementedException();
        }
    }
}
