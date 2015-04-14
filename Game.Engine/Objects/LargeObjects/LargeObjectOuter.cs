namespace Game.Engine.Objects.LargeObjects
{
    abstract class LargeObjectOuterAbstract : FixedObject
    {
        private readonly LargeObjectInner _innerObject;
        private readonly Point _placeInObject;

        public bool IsTransparent { get; private set; }

        public LargeObjectOuterAbstract(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent)
        {
            _innerObject = largeObjectInner;
            _placeInObject = placeInObject;
            IsTransparent = IsTransparent;
            IsPassable = false;
            Size = new Size(1, 1);
        }

        public override string Name
        {
            get { return _innerObject.Name; }
        }

        public override uint GetDrawingCode()
        {
            return _innerObject.Id;
        }
    }
}
