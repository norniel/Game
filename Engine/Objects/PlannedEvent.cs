using System;

namespace Engine.Objects
{
    internal class PlannedEvent: ObjectWithStateBase//, IComparable<PlannedEvent>
    {
        public readonly Func<bool> Act;

        public PlannedEvent(Func<bool> act)
        {
            Act = act;
        }
        /*
        public int CompareTo(PlannedEvent other)
        {
            return base.CompareTo(other);
        }*/
    }
}
