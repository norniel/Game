using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Objects;

namespace Engine
{
    class Tree : FixedObject
    {
        private int _berriesCount = 4;
        private int _branchesCount = 4;

        private int _twigCount = 16;
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

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new CollectBehavior<Berry>(new Berry(), 2, _berriesCount));
            Behaviors.Add(new CollectBehavior<Branch>(new Branch(), 1, _branchesCount));
            Behaviors.Add(new CollectBehavior<Twig>(new Twig(), 2, _twigCount));
        }

        public override string Name => "Tree";

        public override uint GetDrawingCode()
        {
            var berriesCount = (GetBehavior(typeof(CollectBehavior<Berry>)) as CollectBehavior<Berry>).CurrentCount;

            if (berriesCount > _berriesCount / 2) return Id;

            if (berriesCount <= _berriesCount / 2 && berriesCount > 0) return 0x00000200;

            return 0x00000300;
        }

        public virtual int Hardness { get; set; }

   /*     void IHasSmthToCollect<Twig>.SetSmthTotalCount(int totalCount)
        {
            _twigCount = totalCount;

            _branchesCount = (int)Math.Ceiling((double)_twigCount / _twigInBranch) == _branchesCount
                ? _branchesCount
                : (int)Math.Ceiling((double)_twigCount / _twigInBranch);
        }*/
/*
 /*
        void IHasSmthToCollect<Branch>.SetSmthTotalCount(int totalCount)
        {
            _branchesCount = totalCount;

            _twigCount = (int) Math.Ceiling((double) _twigCount/_twigInBranch) == _branchesCount
                ? _twigCount
                : (int) Math.Ceiling((double) _branchesCount*_twigInBranch);
        }*/
    }
}
