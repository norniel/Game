namespace Engine.Interfaces
{
    public interface IMap
    {
        Rect GetSize();

        FixedObject GetObjectFromCell(Point cell);
    }
}
