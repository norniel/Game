namespace Engine.Objects.LargeObjects
{
    abstract class LargeObjectOuterAbstract : FixedObject
    {
        private readonly LargeObjectInner _innerObject;
        public Point PlaceInObject { get; }

        public bool IsTransparent { get; }

        public LargeObjectOuterAbstract(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent)
        {
            _innerObject = largeObjectInner;
            PlaceInObject = placeInObject;
            IsTransparent = isTransparent;
            IsPassable = false;
            Size = new Size(1, 1);

            Name = _innerObject.Name;
        }

        public LargeObjectInner InnerObject => _innerObject;

        public override uint GetDrawingCode()
        {
            return _innerObject.GetDrawingCode();
        }

        public bool isLeftCorner => PlaceInObject != null && PlaceInObject.X == 0 && PlaceInObject.Y == 0;
    }
}
