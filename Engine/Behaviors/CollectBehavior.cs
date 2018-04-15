using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Behaviors
{
    internal class CollectBehavior<T> :IBehavior where T : GameObject
    {
        private readonly T _smth;
        public int PerCollectCount { get; private set; }

        public int TotalCount { get; private set; }

        public int CurrentCount { get; set; }

        public string Name => _smth.Name;

        public GameObject GetSmth()
        {
            return _smth.Clone();
        }

        public CollectBehavior(T smth, int perCollectCount, int totalCount)
        {
            _smth = smth;
            PerCollectCount = perCollectCount;
            TotalCount = totalCount;
            CurrentCount = totalCount;
        }
    }
}
