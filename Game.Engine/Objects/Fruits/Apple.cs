using Game.Engine.Interfaces;
namespace Game.Engine.Objects.Fruits
{
    class Apple: Berry, IEatable
    {
        public override string Name
        {
            get { return "Apple"; }
        }

        public int Poisoness { get { return 0; } }
        public int Satiety { get { return 2; } }
    }
}
