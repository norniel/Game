namespace Engine.Interfaces
{
    interface IEatable : IRemovableObject
    {
        int Poisoness { get; }
        int Satiety { get; }
    }
}
