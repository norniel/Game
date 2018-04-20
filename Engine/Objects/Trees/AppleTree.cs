using Engine.Behaviors;
using Engine.Objects.Fruits;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects.Trees
{
    [GenerateMap]
    class AppleTree : Tree
    {
        public AppleTree()
        {
            Id = 0x00000100;
            Name = "Apple tree";
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();

            Behaviors.RemoveWhere(bv => bv.GetType() == typeof(CollectBehavior<Berry>));
            Behaviors.Add(new CollectBehavior<Berry>(Resource.Apple, 2, 4));
        }
    }
}
