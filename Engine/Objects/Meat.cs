namespace Engine.Objects
{
    class MeatContext : ObjectWithStateContext
    {
        public override string Name { get; set; } = "Meat";

        public override GameObject Produce()
        {
            return new Meat(this);
        }
    }

    class Meat :FixedObjectWithState
    {
        public Meat(ObjectWithStateContext context) : base(context)
        {
        }
    }
}
