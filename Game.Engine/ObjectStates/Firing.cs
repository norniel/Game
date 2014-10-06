using Game.Engine.Interfaces;

namespace Game.Engine.ObjectStates
{
    class Firing:IObjectState
    {
        public int TickCount { get; set; }
        public int Distribution { get; set; }
    }
}
