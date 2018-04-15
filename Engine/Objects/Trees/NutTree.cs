using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;
using Engine.Objects.Fruits;
using Engine.Tools;

namespace Engine
{
    [GenerateMap]
    class NutTree: Tree//, IHasSmthToCollect<Berry>
    {
        public NutTree()
        {
            Id = 0x00002500;
        }

        public override string Name => "Nut tree";

        /*Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Nut();
        }*/

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.RemoveWhere(bv => bv.GetType() == typeof(CollectBehavior<Berry>));
            Behaviors.Add(new CollectBehavior<Berry>(new Nut(), 2, 4));
        }
    }
}