namespace Game.Engine.Interfaces
{
    interface IObjectState
    {
        int TickCount { get; set; }
        int Distribution { get; set; }
        bool Eternal { get; set; }
    }
}
