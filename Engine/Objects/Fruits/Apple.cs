using Engine.Interfaces;
using Engine.Objects.Food;
using Engine.Resources;

namespace Engine.Objects.Fruits
{
    class Apple: Berry, IEatable, IRoastable
    {
        public override int Weight => 2;

        public override string Name => Resource.Apple;

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            this.Properties.Add(Property.Roastable);
        }

        public int Poisoness => 0;
        public int Satiety => 2;

        public GameObject GetRoasted()
        {
            return new RoastedApple();
        }
    }
}
