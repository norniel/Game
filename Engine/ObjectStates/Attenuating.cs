using Engine.Interfaces;

namespace Engine.ObjectStates
{
    class Attenuating:IObjectState
    {
        public int TickCount { get; set; }
        public int Distribution { get; set; }
        public bool Eternal { get; set; }
    }
}
