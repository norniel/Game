using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    [GenerateMap]
    class Bush : FixedObject//, IHasSmthToCollect<Branch>, IHasSmthToCollect<RaspBerries>
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

        public override string Name => Resource.Bush;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Cuttable,
                Property.CollectBerries,
                Property.CollectBranch
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new CollectBehavior<Berry>(new RaspBerries(), 2, _berriesCount));
            Behaviors.Add(new CollectBehavior<Branch>(new Branch(), 2, _branchesCount));
        }
/*
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
        }*/
    }
}
