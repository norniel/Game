using Engine.Behaviors;

namespace Engine.Objects
{
    class RaspBerries : Berry
    {
        public RaspBerries()
        {
            Id = 0x00000900;
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new EatableBehavior(1));
        }

        public override string Name => "Raspberries";

        public override GameObject Clone()
        {
            return new RaspBerries();
        }
    }
}
