using Engine.Objects;

namespace Engine
{
    public class FixedObject : GameObject
    {
        public bool IsPassable { get; set; }

        public Size Size { get; protected set; }

        public int Height { get; set; }

        public FixedObject()
        {
            IsPassable = true;
            Height = 1;
        }

        public FixedObject(Size size, uint id)
        {
            IsPassable = true;
            Height = 1;
            Size = size;
            Id = id;
        }
        
        public override string Name => "Fixed objects";

        public override uint GetDrawingCode()
        {
            return Id;
        }
    }

}
