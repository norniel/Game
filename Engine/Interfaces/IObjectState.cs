namespace Engine.Interfaces
{
    public interface IObjectState
    {
        int TickCount { get; set; }
        int Distribution { get; set; }
        bool Eternal { get; set; }
    }
}

public class ObjStateProperties
{
    public int TickCount { get; set; }
    public int Distribution { get; set; }
    public bool Eternal { get; set; }
}