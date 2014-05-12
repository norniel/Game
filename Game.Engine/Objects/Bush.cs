using System.Collections.Generic;
using Game.Engine.Interfaces;

namespace Game.Engine.Objects
{
    class Bush : FixedObject, IHasSmthToCollect<Branch>, IHasSmthToCollect<RaspBerries>
    {
        private int _initialBerriesCount = 2;
        private int _berriesCount = 2;
        private int _initialBranchesCount = 4;
        private int _branchesCount = 4;

        public Bush()
        {
            IsPassable = false;

            Size = new Size(1, 1);

            Id = 0x00001200;
        }

        public override string Name
        {
            get { return "Bush"; }
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

        int IHasSmthToCollect<Branch>.GetSmthPerCollectCount()
        {
            return 2;
        }

        int IHasSmthToCollect<RaspBerries>.GetSmthTotalCount()
        {
            return _berriesCount;
        }

        void IHasSmthToCollect<RaspBerries>.SetSmthTotalCount(int totalCount)
        {
            _berriesCount = totalCount;
        }

        RaspBerries IHasSmthToCollect<RaspBerries>.GetSmth()
        {
            return new RaspBerries();
        }

        int IHasSmthToCollect<RaspBerries>.GetSmthPerCollectCount()
        {
            return 2;
        }

        int IHasSmthToCollect<Branch>.GetSmthTotalCount()
        {
            return _branchesCount;
        }

        void IHasSmthToCollect<Branch>.SetSmthTotalCount(int totalCount)
        {
            _branchesCount = totalCount;
        }

        Branch IHasSmthToCollect<Branch>.GetSmth()
        {
            return new Branch();
        }
    }
}
