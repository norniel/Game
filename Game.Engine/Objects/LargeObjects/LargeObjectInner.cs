using System;
using System.Collections.Generic;
namespace Game.Engine.Objects.LargeObjects
{
    class LargeObjectInner:FixedObject
    {
        private readonly Lazy<List<LargeObjectOuter>> _lazyObjectsOuter;

        public LargeObjectInner()
        {
            _lazyObjectsOuter = new Lazy<List<LargeObjectOuter>>(InitOuterObjects, true);
        }
  
        protected virtual List<LargeObjectOuter> InitOuterObjects()
        {
            var result = new List<LargeObjectOuter>();
            for (int i = 0; i < this.Size.Width; i++)
            {
                for (int j = 0; j < this.Size.Height; j++)
                {
                    var largeObjectOuter = new LargeObjectOuter(this, new Point(i, j), false);
                    result.Add(largeObjectOuter);
                }                
            }
            return result;
        }      

        public IEnumerable<LargeObjectOuterAbstract> OuterObjects {
            get { return _lazyObjectsOuter.Value; }
        }

        protected class LargeObjectOuter : LargeObjectOuterAbstract
        {
            public LargeObjectOuter(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent) : base(largeObjectInner, placeInObject, isTransparent)
            {
            }
        }
    }
}
