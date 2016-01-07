using System.Linq;

namespace Engine.Objects.LargeObjects.Builder
{
    class TypedBuildItem<T>:BuildItem
    {
        public TypedBuildItem(int percentPerItem)
        {
            CheckObject = (gameObject) => gameObject is T;
            CheckObjects = (objects) => objects.Any(CheckObject);
            PercentPerItem = percentPerItem;
        }
    }
}
