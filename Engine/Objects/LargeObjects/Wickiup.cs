using System.Collections.Generic;
using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.Builder;
using Engine.Objects.LargeObjects.BuilderPlans;
using Engine.Resources;

namespace Engine.Objects
{
    class Wickiup:LargeObjectInner
    {
        public InnerMap _map;

        public bool[,] VerticalBorder;
        public bool[,] HorizontalBorder;

        public Wickiup()
        {
            Size = new Size(2, 3);
            IsPassable = false;
            Id = 0x00001E00;
            _map = new InnerMap((int)Size.Width, (int)Size.Height);

            HorizontalBorder = new bool[Size.Width + 2,2];
            VerticalBorder = new bool[Size.Height, 2];

            for (int i = 0; i < (int)Size.Width + 2; i++)
            {
                HorizontalBorder[i, 0] = true;
                HorizontalBorder[i, 1] = true;
            }
            Name = Resource.Wickiup;
        }
        
        public override uint GetDrawingCode()
        {
            if(IsBuild)
                return Id;

            return Id + BuilderPlan.CurrentDrawingOrder;
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.NeedToBuildWickiup,
               Property.NeedToSleep,
               Property.Enterable
            };
        }
        
        protected override BuilderPlan GetBuilderPlan()
        {
            return new WickiupBuilderPlan();
        }
    }
}
