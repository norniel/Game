using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects.LargeObjects.Builder
{
    class Step
    {
        public List<BuildItemsGroup> ItemGroups { get; set; }

        public bool IsCompleted {
            get { return ItemGroups.All(ig => ig.IsCompleted); }
        }

        public int PercentCompleted {
            get { return ItemGroups.Sum(ig => ig.PercentCompleted)/ItemGroups.Count; }
        }

        public List<GameObject> GetAvailableObjects(IEnumerable<GameObject> gameObjects)
        {
            if (gameObjects == null || !gameObjects.Any())
                return null;

            IEnumerable<GameObject> leftGameObjects = gameObjects;
            List<GameObject> availableObjects = new List<GameObject>();

            foreach (var ig in ItemGroups.Where(ig => !ig.IsCompleted))
            {
                var objectForAction = ig.GetAvailableObjects(leftGameObjects);
                if (objectForAction != null && objectForAction.Any())
                {
                    availableObjects.AddRange(objectForAction);
                    leftGameObjects = leftGameObjects.Except(objectForAction).ToList();
                }

                if (!leftGameObjects.Any())
                    break;
            };

            return availableObjects;
        }

        public List<GameObject> BuildAction(List<GameObject> gameObjects)
        {
            if (gameObjects == null || !gameObjects.Any())
                return gameObjects;

            List<GameObject> leftGameObjects = gameObjects;
            foreach (var ig in ItemGroups.Where(ig => !ig.IsCompleted))
            {
                leftGameObjects = ig.Build(leftGameObjects);

                if (!leftGameObjects.Any())
                    break;
            };
            
            return leftGameObjects;
        }
    }
}
