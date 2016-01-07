using System.Collections.Generic;
using Engine.Objects;
using Engine.Objects.LargeObjects.Builder;

namespace Engine.Objects.LargeObjects.BuilderPlans
{
    class GrassBedBuilderPlan:BuilderPlan
    {
        const int percentPerItem = 10;
        public GrassBedBuilderPlan()
        {
            Steps = new List<Step>
            {
                new Step()
                {
                    ItemGroups = new List<BuildItemsGroup>
                    {
                        new BuildItemsGroup()
                        {
                            PercentPerAction = 20,
                            BuildItems = new List<BuildItem>()
                            {
                                new TypedBuildItem<Plant>(percentPerItem)
                            }
                        }
                    }
                }
            };
        }
    }
}
