using Engine.Behaviors;
using Engine.Objects.Food;
using Engine.Resources;

namespace Engine.Objects.Fruits
{
    class Apple: Berry
    {
        public override int Weight => 2;

        public override string Name => Resource.Apple;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            Properties.Add(Property.Roastable);
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new RoastBehavior(new RoastedApple()));
            Behaviors.Add(new EatableBehavior(2));
        }

        public override GameObject Clone()
        {
            return new Apple();
        }
    }
}
