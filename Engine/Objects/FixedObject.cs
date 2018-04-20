using Engine.Interfaces;
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
            Size = new Size(1, 1);

            Name = "Fixed objects";
        }

        public FixedObject(Size size, uint id)
        {
            IsPassable = true;
            Height = 1;
            Size = size;
            Id = id;

            Name = "Fixed objects";
        }

        public FixedObject(IObjectContext context) : base(context)
        {
            IsPassable = true;
            Height = 1;
            Size = new Size(1, 1);
        }

        public override uint GetDrawingCode()
        {
            return Id;
        }
    }

}
