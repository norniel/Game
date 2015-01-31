using Game.Engine.Interfaces;

namespace Game.Engine.ObjectStates
{
    class Growing : IObjectState
    {
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
    }
}
