using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Tools;

namespace Engine.Objects.Trees
{
    [GenerateMap]
    class AppleTree : Tree, IHasSmthToCollect<Berry>
    {
        private int _initialBerriesCount = 4;
        private int _berriesCount = 4;
        private int _initialBranchesCount = 6;
        private int _branchesCount = 6;

        public AppleTree()
        {
            Id = 0x00000100;
        }

        public override string Name => "Apple tree";

        Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Apple();
        }
    }
}
