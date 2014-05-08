using System.Collections.Generic;
using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine
{
    class Tree : FixedObject, IHasSmthToCollect<Branch>, IHasSmthToCollect<Berry>
    {
        private int _initialBerriesCount = 4;
        private int _berriesCount = 4;
        private int _initialBranchesCount = 6;
        private int _branchesCount = 6;

        public Tree()
        {
            IsPassable = false;

            Size = new Size( 1, 1 );

            Id = 0x00000100;
        }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
                Property.Cuttable,
                Property.CollectBerries,
                Property.CollectBranch
            };
        }
        /*
        public int BerriesPerCollectCount {
            get { return 2; }
            set { }
        }
        public int BerriesCount
        {
            get { return _berriesCount; }
            set { _berriesCount = value; }
        }
        public Berry GetBerry()
        {
            return new Berry();
        }
        */
        public override string Name
        {
            get { return "Tree"; }
        }

        public override uint GetDrawingCode()
        {
            var berriesCount = ((IHasSmthToCollect<Berry>) this).GetSmthTotalCount();

            if (_berriesCount > this._initialBerriesCount / 2) return this.Id;

            if (berriesCount <= this._initialBerriesCount / 2 && berriesCount > 0) return 0x00000200;

            return 0x00000300;
        }
/*
        public int BranchesPerCollectCount {
            get { return 2; }
        }
        public int BranchesCount {
            get { return _branchesCount; }
            set { _branchesCount = value; }
        }
        public Branch GetBranch()
        {
            return new Branch();
        }
        */
        int IHasSmthToCollect<Branch>.GetSmthPerCollectCount()
        {
            return 2;
        }

        int IHasSmthToCollect<Berry>.GetSmthTotalCount()
        {
            return _berriesCount;
        }

        void IHasSmthToCollect<Berry>.SetSmthTotalCount(int totalCount)
        {
            _berriesCount = totalCount;
        }

        void IHasSmthToCollect<Branch>.SetSmthTotalCount(int totalCount)
        {
            _branchesCount = totalCount;
        }

        Berry IHasSmthToCollect<Berry>.GetSmth()
        {
            return new Berry();
        }

        int IHasSmthToCollect<Berry>.GetSmthPerCollectCount()
        {
            return 2;
        }

        int IHasSmthToCollect<Branch>.GetSmthTotalCount()
        {
            return _branchesCount;
        }

        Branch IHasSmthToCollect<Branch>.GetSmth()
        {
            return new Branch();
        }
    }
}
