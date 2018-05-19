using System;

namespace Engine.Objects
{
    internal class PlannedEvent: ObjectWithStateBase
    {
        public readonly Func<bool> Act;

        public PlannedEvent(Func<bool> act)
        {
            Act = act;
        }
    }
}
