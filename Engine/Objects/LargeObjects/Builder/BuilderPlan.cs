using System.Collections.Generic;
using System.Linq;

namespace Engine.Objects.LargeObjects.Builder
{
    class BuilderPlan
    {
        public bool IsCompleted {
            get { return Steps.All(s => s.IsCompleted); }
        }

        private int _currentStepIndex = 0;
        
        public List<Step> Steps = new List<Step>();

        public Step CurrentStep {
            get { return Steps[_currentStepIndex]; }
        }

        public void MoveNextStep()
        {
            _currentStepIndex = _currentStepIndex + 1 < Steps.Count ? _currentStepIndex + 1 : _currentStepIndex;
        }
    }
}
