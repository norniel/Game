using Engine.Interfaces;
using Engine.ObjectStates;

namespace Engine.Behaviors
{
    class SpoileringBehavior: IBehavior
    {
        public ObjStateProperties SpoileringProps { get; set; } = new ObjStateProperties() { TickCount = 500, Distribution = 100, Eternal = false };
        public ObjStateProperties StayingProps { get; set; } = new ObjStateProperties() { TickCount = 500, Distribution = 100, Eternal = false };

        public int Poisonness { get; set; } = 0;

    }
}
