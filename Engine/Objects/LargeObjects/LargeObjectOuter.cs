namespace Engine.Objects.LargeObjects
{
    abstract class LargeObjectOuterAbstract : FixedObject
    {
        private readonly LargeObjectInner _innerObject;
        public Point PlaceInObject { get; private set; }

        public bool IsTransparent { get; private set; }

        public LargeObjectOuterAbstract(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent)
        {
            _innerObject = largeObjectInner;
            PlaceInObject = placeInObject;
            IsTransparent = IsTransparent;
            IsPassable = false;
            Size = new Size(1, 1);
        }

        public LargeObjectInner InnerObject { get { return _innerObject; } }

        public override string Name
        {
            get { return _innerObject.Name; }
        }

        public override uint GetDrawingCode()
        {
            return _innerObject.GetDrawingCode();
        }

        public bool isLeftCorner { get { return PlaceInObject != null && PlaceInObject.X == 0 && PlaceInObject.Y == 0; } }
    }
}
