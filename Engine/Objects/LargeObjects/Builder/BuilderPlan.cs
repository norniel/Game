using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects.LargeObjects.Builder
{
    abstract class BuilderPlan
    {
        public bool IsCompleted {
            get { return Steps.All(s => s.IsCompleted); }
        }

        protected int _currentStepIndex;
        
        public List<Step> Steps = new List<Step>();

        public Step CurrentStep => Steps[_currentStepIndex];

        public abstract string Name { get; }

        public void MoveNextStep()
        {
            _currentStepIndex = _currentStepIndex + 1 < Steps.Count ? _currentStepIndex + 1 : _currentStepIndex;
        }

        public abstract bool CheckAvailablePlace(Point cell);

        public abstract uint CurrentDrawingOrder{get;}
    }
}
