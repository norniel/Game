using System.Collections.Generic;
using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.Builder;
using Engine.Objects.LargeObjects.BuilderPlans;
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
        
        public override string Name
        {
            get { return Resource.Wickiup; }
        }

        public override uint GetDrawingCode()
        {
            if(IsBuild)
                return this.Id;

            return this.Id + BuilderPlan.CurrentDrawingOrder;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.NeedToBuildWickiup,
               Property.NeedToSleep
            };
        }
        
        protected override BuilderPlan GetBuilderPlan()
        {
            return new WickiupBuilderPlan();
        }
    }
}
