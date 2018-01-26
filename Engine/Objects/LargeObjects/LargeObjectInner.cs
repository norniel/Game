using System;
using System.Collections.Generic;
using Engine.Objects.LargeObjects.Builder;

namespace Engine.Objects.LargeObjects
{
    abstract class LargeObjectInner:FixedObject
    {
        private readonly Lazy<List<LargeObjectOuter>> _lazyObjectsOuter;

        private readonly Lazy<BuilderPlan> _builderPlanLazy;

        private readonly Lazy<Consist> _consist = new Lazy<Consist>(() => new Consist());

        public BuilderPlan BuilderPlan => _builderPlanLazy.Value;

        public LargeObjectInner()
        {
            _lazyObjectsOuter = new Lazy<List<LargeObjectOuter>>(InitOuterObjects, true);
            _builderPlanLazy = new Lazy<BuilderPlan>(GetBuilderPlan, true);
            RemoveFromContainer = (() =>
            {
                foreach(var oo in OuterObjects) oo.RemoveFromContainer();
            });
        }
  
        protected virtual List<LargeObjectOuter> InitOuterObjects()
        {
            var result = new List<LargeObjectOuter>();
            for (int i = 0; i < Size.Width; i++)
            {
                for (int j = 0; j < Size.Height; j++)
                {
                    var largeObjectOuter = new LargeObjectOuter(this, new Point(i, j), false);
                    result.Add(largeObjectOuter);
                }                
            }
            return result;
        }

        protected abstract BuilderPlan GetBuilderPlan();

        public IEnumerable<LargeObjectOuterAbstract> OuterObjects => _lazyObjectsOuter.Value;

        public bool IsBuild => BuilderPlan.IsCompleted;

        protected class LargeObjectOuter : LargeObjectOuterAbstract
        {
            public LargeObjectOuter(LargeObjectInner largeObjectInner, Point placeInObject, bool isTransparent) : base(largeObjectInner, placeInObject, isTransparent)
            {
            }
        }
    }
}
