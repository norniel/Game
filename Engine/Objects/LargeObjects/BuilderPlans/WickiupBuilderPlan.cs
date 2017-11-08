using System;
using System.Collections.Generic;
using Engine.Objects.LargeObjects.Builder;
using Engine.Resources;

namespace Engine.Objects.LargeObjects.BuilderPlans
{
    class WickiupBuilderPlan : BuilderPlan
    {
        public WickiupBuilderPlan()
        {
            Steps = new List<Step>
            {
                new Step()
                {
                    ItemGroups = new List<BuildItemsGroup>
                    {
                        new BuildItemsGroup()
                        {
                            PercentPerAction = 34,
                            BuildItems = new List<BuildItem>()
                            {
                                new TypedBuildItem<Branch>(34)
                            }
                        }
                    }
                },
                new Step()
                {
                    ItemGroups = new List<BuildItemsGroup>
                    {
                        new BuildItemsGroup()
                        {
                            PercentPerAction = 21,
                            BuildItems = new List<BuildItem>()
                            {
                                new TypedBuildItem<Branch>(7)
                            }
                        },
                        new BuildItemsGroup()
                        {
                            PercentPerAction = 21,
                            BuildItems = new List<BuildItem>()
                            {
                                new TypedBuildItem<Root>(7)
                            }
                        }
                    }
                },
                new Step()
                {
                    ItemGroups = new List<BuildItemsGroup>
                    {
                        new BuildItemsGroup()
                        {
                            PercentPerAction = 20,
                            BuildItems = new List<BuildItem>()
                            {
                                new TypedBuildItem<Plant>(4),
                                new TypedBuildItem<Branch>(10)
                            }
                        }
                    }
                }
            };
        }

        public override string Name => ActionsResource.CreateWickiup;

        public override bool CheckAvailablePlace(Point cell)
        {
            var mapSize = Game.Map.GetSize();

            if (cell.Y + 1 >= mapSize.Height || cell.Y + 1 >= mapSize.Height || cell.X + 1 >= mapSize.Width)
                return false;

            for (int cx = 0; cx < 2 ; cx++)
            {
                for (int cy = 0; cy < 3; cy++)
                {
                    var objectOnPlace = Game.Map.GetHObjectFromCell(new Point(cell.X + cx, cell.Y + cy));
                    if (objectOnPlace != null)
                        return false;
                }
            }
            
            return true;
        }

        public override uint CurrentDrawingOrder => (uint)(_currentStepIndex*2 + (CurrentStep.PercentCompleted < 50 ? 1 : 2));
    }
}
