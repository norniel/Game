using Engine.Objects;

namespace Engine.Interfaces
{
    interface IHasSmthToCollect<out T> where T: GameObject
    {
        int GetSmthPerCollectCount();

        int GetSmthTotalCount();

        void SetSmthTotalCount(int totalCount);

        T GetSmth();
    }
}
