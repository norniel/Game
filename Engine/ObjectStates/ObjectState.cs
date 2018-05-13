namespace Engine.ObjectStates
{
    public class ObjectState
    {
        public ObjectStates Name { get; set; }
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
        public uint? Id { get; set; }

        public ObjectState(ObjectStates name)
        {
            Name = name;
        }

        public ObjectState(ObjectStates name, ObjStateProperties props): this(name)
        {
            TickCount = props.TickCount;
            Distribution = props.Distribution;
            Eternal = props.Eternal;
            Id = props.Id;
        }

        public ObjectState(ObjectStates name, ObjStateProperties props, uint? id) : this(name)
        {
            TickCount = props.TickCount;
            Distribution = props.Distribution;
            Eternal = props.Eternal;
            Id = id;
        }
    }

    public class ObjStateProperties
    {
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
        public uint? Id { get; set; }
    }
}
