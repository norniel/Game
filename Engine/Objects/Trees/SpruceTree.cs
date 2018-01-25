using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Tools;

namespace Engine.Objects.Trees
{
    [GenerateMap]
    class SpruceTree : Tree, IHasSmthToCollect<Berry>, IHasSmthToCollect<Root>
    {
        private int _rootCount = 4;

        public SpruceTree()
        {
            Id = 0x00001600;
        }

        public override string Name => "Spruce tree";

        public override uint GetDrawingCode()
        {
            return Id;
        }

        int IHasSmthToCollect<Root>.GetSmthPerCollectCount()
        {
            return 1;
        }

        int IHasSmthToCollect<Root>.GetSmthTotalCount()
        {
            return _rootCount;
        }

        void IHasSmthToCollect<Root>.SetSmthTotalCount(int totalCount)
        {
            _rootCount = totalCount;
        }

        Root IHasSmthToCollect<Root>.GetSmth()
        {
            return new Root();
        }

        Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Cone();
        }

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            Properties.Add(Property.CollectRoot);
        }
    }
}
