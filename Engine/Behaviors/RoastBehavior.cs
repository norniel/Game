using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Behaviors
{
    class RoastBehavior : IBehavior
    {
        private readonly GameObject _roasted;
        public RoastBehavior(GameObject roasted)
        {
            _roasted = roasted;
        }

        public GameObject GetRoasted()
        {
            return _roasted.Clone();
        }
    }
}
