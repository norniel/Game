using Engine.Behaviors;
using Engine.Objects;

namespace Engine.States
{
    class ShakingTree:IState
    {
        protected readonly MobileObject _mobileObject;
        private readonly CollectBehavior<Berry> TreeBehavior;
        private const int MAX_MAXSHAKINGTICK = 20;
        private int _shakingTicks;
        private readonly int _maxRestingTicks;
        public ShakingTree(MobileObject mobileObject, CollectBehavior<Berry> treeBehavior)
        {
            _mobileObject = mobileObject;
            _maxRestingTicks = Game.Random.Next(MAX_MAXSHAKINGTICK);
            TreeBehavior = treeBehavior;
        }

        public void Act()
        {
            if (TreeBehavior == null || TreeBehavior.CurrentCount <= 0)
            {
                _mobileObject.StateEvent.FireEvent();
                return;
            }

            if (_maxRestingTicks <= _shakingTicks)
            {
                TreeBehavior.CurrentCount = TreeBehavior.CurrentCount - 1;
                var fruit = TreeBehavior.GetSmth();
                var destCell = Game.Map.GetNearestRandomEmptyCellFromPoint(_mobileObject.Position);

                if(destCell != null)
                    Game.Map.SetObjectFromCell(destCell, fruit as FixedObject);

                _mobileObject.StateEvent.FireEvent();
                return;
            }

            _shakingTicks++;
        }

        public bool ShowActing => false;
    }
}
