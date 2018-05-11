using Engine.Interfaces;

namespace Engine.ObjectStates
{
    public class ObjectState : IObjectState
    {
        public ObjectStates Name { get; set; }
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }

        public ObjectState(ObjectStates name)
        {
            Name = name;
        }

        public ObjectState(ObjectStates name, ObjStateProperties props): this(name)
        {
            TickCount = props.TickCount;
            Distribution = props.Distribution;
            Eternal = props.Eternal;
        }
    }
}
