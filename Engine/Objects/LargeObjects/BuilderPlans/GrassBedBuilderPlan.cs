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

        public override string Name {
            get { return ActionsResource.BuildGrassBed; }
        }
        public override bool CheckAvailablePlace(Point cell)
        {
            var mapSize = Game.Map.GetSize();

            if (cell.Y + 1 >= mapSize.Height)
                return false;

            var objectOnPlace = Game.Map.GetObjectFromCell(cell);
            var objectOnNextPlace = Game.Map.GetObjectFromCell(new Point(cell.X, cell.Y + 1));

            return (objectOnPlace == null && objectOnNextPlace == null);
        }
    }
}
