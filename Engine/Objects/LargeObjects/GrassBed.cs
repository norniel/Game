using System.Collections.Generic;
using Engine.Objects.LargeObjects.Builder;
using Engine.Objects.LargeObjects.BuilderPlans;

namespace Engine.Objects.LargeObjects
{
    class GrassBed:LargeObjectInner
    {
        public GrassBed()
        {
            Size = new Size(1, 2);
            IsPassable = false;
            Id = 0x00001D00;
        }

        public override string Name
        {
            get { return "Grass bed"; }
        }

        public override uint GetDrawingCode()
        {
            if(IsBuild)
                return this.Id;

            var buildProcents = BuilderPlan.CurrentStep.PercentCompleted;//CountLeftToBuild * 100 / CountToBuild;
            if (buildProcents <= 30)
            {
                return 0x00001D03;
            }
            else if (buildProcents <= 60)
            {
                return 0x00001D02;
            }

            return 0x00001D01;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.NeedToBuildGrassBed,
               Property.NeedToSleep
            };
        }

        
        protected override BuilderPlan GetBuilderPlan()
        {
            return new GrassBedBuilderPlan();
        }
    }
}
