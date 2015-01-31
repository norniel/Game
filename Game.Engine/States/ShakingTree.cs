using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine.States
{
    class ShakingTree:IState
    {
        protected readonly MobileObject _mobileObject;
        private readonly IHasSmthToCollect<Berry> Tree;
        private const int MAX_MAXSHAKINGTICK = 20;
        private int _shakingTicks = 0;
        private readonly int _maxRestingTicks;
        public ShakingTree(MobileObject mobileObject, IHasSmthToCollect<Berry> tree)
        {
            _mobileObject = mobileObject;
            _maxRestingTicks = Game.Random.Next(MAX_MAXSHAKINGTICK);
            Tree = tree;
        }

        public void Act()
        {
            if (Tree.GetSmthTotalCount() <= 0)
            {
                _mobileObject.StateEvent.FireEvent();
                return;
            }

            if (_maxRestingTicks <= _shakingTicks)
            {
                Tree.SetSmthTotalCount(Tree.GetSmthTotalCount() - 1);
                var fruit = Tree.GetSmth();
                var destCell = Game.Map.GetNearestRandomEmptyCellFromPoint(_mobileObject.Position);

                if(destCell != null)
                    Game.Map.SetObjectFromCell(destCell, fruit);

                _mobileObject.StateEvent.FireEvent();
                return;
            }

            _shakingTicks++;
        }

        public bool ShowActing { get { return false; } }
    }
}
