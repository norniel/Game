using Game.Engine.Interfaces;
using Game.Engine.Objects.Food;

namespace Game.Engine.Objects.Fruits
{
    class Apple: Berry, IEatable, IRoastable
    {
        public override int Weight { get { return 2; } }

        public override string Name
        {
            get { return "Apple"; }
        }

        public override void InitializeProperties()
        {
            base.InitializeProperties();
            this.Properties.Add(Property.Roastable);
        }

        public int Poisoness { get { return 0; } }
        public int Satiety { get { return 2; } }
        public GameObject GetRoasted()
        {
            return new RoastedApple();
        }
    }
}
