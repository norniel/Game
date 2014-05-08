namespace Game.Engine.Interfaces
{
    using Objects;
    
    interface IHasSmthToCollect<T> where T: GameObject
    {
        int GetSmthPerCollectCount();

        int GetSmthTotalCount();

        void SetSmthTotalCount(int totalCount);

        T GetSmth();
    }
}
