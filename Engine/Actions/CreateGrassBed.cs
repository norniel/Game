using Engine.Objects.LargeObjects;
using Engine.Objects.LargeObjects.BuilderPlans;

namespace Engine.Actions
{
    class CreateGrassBed : CreateObjectWithPlan<GrassBed, GrassBedBuilderPlan>
    {
        public override bool IsApplicable(Property property)
        {
            return property == Property.NeedToBuildGrassBed;
        }
    }
}
