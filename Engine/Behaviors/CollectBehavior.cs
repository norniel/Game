using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Behaviors
{
    internal class CollectBehavior<T> :IBehavior where T : GameObject
    {
        public int PerCollectCount { get; private set; }

        public int TotalCount { get; private set; }

        public int CurrentCount { get; set; }

        public string Name { get; set; }

        public GameObject GetSmth()
        {
            return Game.Factory.Produce(Name);
        }

        public CollectBehavior(string name, int perCollectCount, int totalCount)
        {
            Name = name;
            PerCollectCount = perCollectCount;
            TotalCount = totalCount;
            CurrentCount = totalCount;
        }
    }
}
