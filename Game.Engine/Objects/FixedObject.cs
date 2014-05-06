using Game.Engine.Objects;

namespace Game.Engine
{
    class FixedObject : GameObject
    {
        public bool IsPassable { get; protected set; }

        public Size Size { get; protected set; }

        public uint Id { get; protected set; }

        public FixedObject()
        {
            IsPassable = true;
        }

        public void Draw(){}

        public override string Name
        {
            get { return "Fixed objects"; }
        }

        public override uint GetDrawingCode()
        {
            return this.Id;
        }
    }

}
