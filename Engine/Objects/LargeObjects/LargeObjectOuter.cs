namespace Engine.Objects.LargeObjects
{
    abstract class LargeObjectOuterAbstract : FixedObject
    {
        private readonly LargeObjectInner _innerObject;
        public Point PlaceInObject { get; }

        public bool IsTransparent { get; }
        public bool IsEvenSized { get; }
        
        public bool IsCenterDown { get; }
        public LargeObjectOuterAbstract(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent, bool isCenterDown, bool isEvenSized)
        {
            _innerObject = largeObjectInner;
            PlaceInObject = placeInObject;
            IsTransparent = isTransparent;
            IsCenterDown = isCenterDown;
            IsEvenSized = isEvenSized;
            IsPassable = false;
            Size = new Size(1, 1);

            Name = _innerObject.Name;
        }

        public LargeObjectInner InnerObject => _innerObject;

        public override uint GetDrawingCode()
        {
            return _innerObject.GetDrawingCode();
        }
        
    }
}
