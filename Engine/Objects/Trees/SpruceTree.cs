using Engine.Behaviors;
using Engine.Objects.Fruits;
using Engine.Tools;

namespace Engine.Objects.Trees
{
    [GenerateMap]
    class SpruceTree : Tree
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

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            Properties.Add(Property.CollectRoot);
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.RemoveWhere(bv => bv.GetType() == typeof(CollectBehavior<Berry>));
            Behaviors.Add(new CollectBehavior<Berry>(new Cone(), 2, 4));
            Behaviors.Add(new CollectBehavior<Root>(new Root(), 1, _rootCount));
        }
    }
}
