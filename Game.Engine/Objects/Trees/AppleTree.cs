using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.Objects.Fruits;

namespace Game.Engine.Objects.Trees
{
    class AppleTree : Tree, IHasSmthToCollect<Branch>, IHasSmthToCollect<Berry>
    {
        private int _initialBerriesCount = 4;
        private int _berriesCount = 4;
        private int _initialBranchesCount = 6;
        private int _branchesCount = 6;

        public AppleTree()
        {
            Id = 0x00000100;
        }

        public override string Name
        {
            get { return "Apple tree"; }
        }

        public int Hardness { get; set; }

        Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Apple();
        }

        Branch IHasSmthToCollect<Branch>.GetSmth()
        {
            return new Branch();
        }
    }
}
