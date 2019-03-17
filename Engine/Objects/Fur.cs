namespace Engine.Objects
{
    class FurContext : ObjectWithStateContext
    {
        public override string Name { get; set; } = "Fur";
        public override GameObject Produce()
        {
            return new Fur(this);
        }
    }
    class Fur:FixedObjectWithState
    {
        public Fur(ObjectWithStateContext context) : base(context)
        {
        }
    }
}
