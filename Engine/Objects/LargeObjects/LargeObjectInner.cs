using System;
using System.Collections.Generic;
using Engine.Objects.LargeObjects.Builder;
using Microsoft.Practices.ObjectBuilder2;

namespace Engine.Objects.LargeObjects
{
    abstract class LargeObjectInner:FixedObject
    {
        private readonly Lazy<List<LargeObjectOuter>> _lazyObjectsOuter;

        private readonly Lazy<BuilderPlan> _builderPlanLazy;

        public BuilderPlan BuilderPlan { get; set; }

        public LargeObjectInner()
        {
            _lazyObjectsOuter = new Lazy<List<LargeObjectOuter>>(InitOuterObjects, true);
            _builderPlanLazy = new Lazy<BuilderPlan>(GetBuilderPlan, true);
            RemoveFromContainer = (() =>
            {
                OuterObjects.ForEach(oo => oo.RemoveFromContainer());
            });
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

        protected abstract BuilderPlan GetBuilderPlan();

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
