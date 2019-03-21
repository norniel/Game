using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects.LargeObjects.Builder
{
    public class BuildItemsGroup
    {
        private int _percentLeftToBuild = 100;
        public int PercentPerAction { get; set; }
        public List<BuildItem> BuildItems { get; set; }
        public bool IsCompleted => _percentLeftToBuild <= 0;

        public int PercentCompleted => _percentLeftToBuild <= 0 ? 100 : 100 - _percentLeftToBuild;

        public List<GameObject> Build(IEnumerable<GameObject> gameObjects, int tiredness)
        {
            if (IsCompleted)
                return gameObjects.ToList();
            
            var availableObjects = new List<GameObject>();

            foreach (var buildItem in BuildItems)
            {
                if (_percentLeftToBuild <= 0)
                    break;

                var percentPerBuild = Math.Min(_percentLeftToBuild, PercentPerAction);

                var buildItemCount = percentPerBuild / buildItem.PercentPerItem;
                buildItemCount = buildItemCount * buildItem.PercentPerItem < percentPerBuild ? buildItemCount + 1 : buildItemCount;
                var buildItemAVObjects = gameObjects.Where(lgo => buildItem.CheckObject(lgo)).Take(buildItemCount).ToList();
                availableObjects.AddRange(buildItemAVObjects);

                var itemsToBuildCount = buildItemAVObjects.Count;
                buildItemAVObjects.OrderBy(gobj => gobj.Quality);
                buildItem.CountUsedToBuild += itemsToBuildCount;

                _percentLeftToBuild = _percentLeftToBuild - itemsToBuildCount * buildItem.PercentPerItem;
            }

            availableObjects.ForEach(go => go.RemoveFromContainer?.Invoke());

            return gameObjects.Except(availableObjects).ToList();
        }

        public List<GameObject> GetAvailableObjects(IEnumerable<GameObject> leftGameObjects)
        {
            var currentPercentLeft = _percentLeftToBuild;
            var availableObjects = new List<GameObject>();

            foreach (var buildItem in BuildItems)
            {
                if (currentPercentLeft <= 0)
                    break;

                var buildItemCount = currentPercentLeft / buildItem.PercentPerItem;
                buildItemCount = buildItemCount* buildItem.PercentPerItem < currentPercentLeft ? buildItemCount + 1 : buildItemCount;
                var buildItemAVObjects = leftGameObjects.Where(lgo => buildItem.CheckObject(lgo)).Take(buildItemCount).ToList();
                availableObjects.AddRange(buildItemAVObjects);

                currentPercentLeft = currentPercentLeft - buildItemAVObjects.Count*buildItem.PercentPerItem;
            }

            return availableObjects;
        }
    }
}
