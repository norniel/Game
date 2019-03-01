using Engine.Interfaces;
using Engine.ObjectStates;

namespace Engine.Behaviors
{
    class SpoileringBehavior: IBehavior
    {
        public ObjStateProperties SpoileringProps { get; set; } = new ObjStateProperties { TickCount = DayNightCycle.HalfDayLength/2, Distribution = DayNightCycle.HalfDayLength / 10, Eternal = false };
        public ObjStateProperties StayingProps { get; set; } = new ObjStateProperties { TickCount = DayNightCycle.HalfDayLength / 2, Distribution = DayNightCycle.HalfDayLength / 10, Eternal = false };

        public int Poisonness { get; set; } = 0;

    }
}
