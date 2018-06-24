namespace Engine.ObjectStates
{
    public class ObjectState
    {
        public ObjectStates Name { get; set; }
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
        public uint? Id { get; set; }
        public uint? BaseId { get; set; }

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
            BaseId = props.BaseId;
        }

        public ObjectState(ObjectStates name, ObjStateProperties props, uint? id, uint? baseId = null) : this(name)
        {
            TickCount = props.TickCount;
            Distribution = props.Distribution;
            Eternal = props.Eternal;
            Id = id;
            BaseId = baseId;
        }
    }

    public class ObjStateProperties
    {
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
        public uint? Id { get; set; }
        public uint? BaseId { get; set; }
    }
}
