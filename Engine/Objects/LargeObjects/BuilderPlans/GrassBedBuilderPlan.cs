using System.Collections.Generic;
using Engine.Objects.LargeObjects.Builder;
using Engine.Resources;

namespace Engine.Objects.LargeObjects.BuilderPlans
{
    class GrassBedBuilderPlan:BuilderPlan
    {
        const int percentPerItem = 10;
        public GrassBedBuilderPlan()
        {
            Steps = new List<Step>
            {
                new Step
                {
                    ItemGroups = new List<BuildItemsGroup>
                    {
                        new BuildItemsGroup
                        {
                            PercentPerAction = 20,
                            BuildItems = new List<BuildItem>
                            {
                                new TypedBuildItem<Plant>(percentPerItem)
                            }
                        }
                    }
                }
            };
        }

        public override string Name => ActionsResource.BuildGrassBed;

        public override bool CheckAvailablePlace(Point cell)
        {
            var mapSize = Game.Map.GetSize();

            if (cell.Y + 1 >= mapSize.Height)
                return false;

            var objectOnPlace = Game.Map.GetHObjectFromCell(cell);
            var objectOnNextPlace = Game.Map.GetHObjectFromCell(new Point(cell.X, cell.Y + 1));

            return objectOnPlace == null && objectOnNextPlace == null;
        }

        public override uint CurrentDrawingOrder {
            get
            {
                var buildProcents = CurrentStep.PercentCompleted; //CountLeftToBuild * 100 / CountToBuild;
                if (buildProcents <= 30)
                {
                    return 0x3;
                }

                if (buildProcents <= 60)
                {
                    return 0x2;
                }
                return 0x1;
            }
        }
    }
}
