using Game.Engine.Interfaces;
using Game.Engine.Objects.Fruits;

namespace Game.Engine.Objects.Trees
{
    class SpruceTree : Tree, IHasSmthToCollect<Berry>
    {
        public SpruceTree()
        {
            Id = 0x00001600;
        }

        public override string Name
        {
            get { return "Spruce tree"; }
        }

        public override uint GetDrawingCode()
        {
            return Id;
        }

        Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Cone();
        }
    }
}
