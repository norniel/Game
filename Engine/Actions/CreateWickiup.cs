using Engine.Objects;
using Engine.Objects.LargeObjects.BuilderPlans;

namespace Engine.Actions
{
    class CreateWickiup:CreateObjectWithPlan<Wickiup, WickiupBuilderPlan>
    {
        public override bool IsApplicable(Property property)
        {
            return Property.NeedToBuildWickiup == property;
        }
    }
}
