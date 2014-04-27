using Game.Engine.Objects;

namespace Game.Engine
{
    public abstract class MobileObject : GameObject
    {
        public Point Position { get; set; }

        public override string Name
        {
            get { return "Mobile object"; }
        }
    }
}
