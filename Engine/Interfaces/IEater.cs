namespace Engine.Interfaces
{
    interface IEater
    {
        void Eat(int satiety);
        EaterType EaterType { get; }
    }
}
