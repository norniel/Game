using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    class Tree : FixedObject, IHasSmthToCollect<Branch>, IHasSmthToCollect<Berry>, IHasSmthToCollect<Twig>
    {
        private const int _initialBerriesCount = 4;
        private int _berriesCount = 4;
        private const int _initialBranchesCount = 4;
        private int _branchesCount = 4;

        private int _twigCount = 16;

        private const int _twigInBranch = 4;

        public override int Height => 2;
        
        public Tree()
        {
            IsPassable = false;

            Size = new Size( 1, 1 );

            Id = 0x00000100;
        }

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
                Property.Cuttable,
                Property.CollectBerries,
                Property.CollectBranch,
                Property.CollectTwig
            };
        }
        
        public override string Name => "Tree";

        public override uint GetDrawingCode()
        {
            var berriesCount = ((IHasSmthToCollect<Berry>) this).GetSmthTotalCount();

            if (_berriesCount > _initialBerriesCount / 2) return Id;

            if (berriesCount <= _initialBerriesCount / 2 && berriesCount > 0) return 0x00000200;

            return 0x00000300;
        }

        public virtual int Hardness { get; set; }

        int IHasSmthToCollect<Branch>.GetSmthPerCollectCount()
        {
            return 1;
        }

        int IHasSmthToCollect<Twig>.GetSmthTotalCount()
        {
            return _twigCount;
        }

        void IHasSmthToCollect<Twig>.SetSmthTotalCount(int totalCount)
        {
            _twigCount = totalCount;

            _branchesCount = (int)Math.Ceiling((double)_twigCount / _twigInBranch) == _branchesCount
                ? _branchesCount
                : (int)Math.Ceiling((double)_twigCount / _twigInBranch);
        }

        Twig IHasSmthToCollect<Twig>.GetSmth()
        {
            return new Twig();
        }

        int IHasSmthToCollect<Twig>.GetSmthPerCollectCount()
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

            _twigCount = (int) Math.Ceiling((double) _twigCount/_twigInBranch) == _branchesCount
                ? _twigCount
                : (int) Math.Ceiling((double) _branchesCount*_twigInBranch);
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
