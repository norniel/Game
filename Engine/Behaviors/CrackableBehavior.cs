using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Behaviors
{
    internal class CrackableBehavior: IBehavior
    {
        private readonly string _name;
        public CrackableBehavior(string crackableName)
        {
            _name = crackableName;
        }

        public GameObject GetCrackable()
        {
            return Game.Factory.Produce(_name);
        }
    }
}
