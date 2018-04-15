using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects.Fruits;
using Engine.Resources;

namespace Engine.Objects
{
    class Branch : FixedObject//, IHasSmthToCollect<Twig>
    {
        private int _twigCount = 4;

        public Branch() 
        {
            IsPassable = true;

            Size = new Size(1, 1);

            Id = 0x00000800;
        }

        public override int Weight => 5;

        public override void InitializeProperties()
        {
            Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.NeedToCreateStoneAxe,
               Property.NeedToMakeFireWithWood,
               Property.CollectTwig,
               Property.Branch,
               Property.NeedToBuildWickiup
            };
        }

        public override void InitializeBehaviors()
        {
            base.InitializeBehaviors();
            Behaviors.Add(new BurnableBehavior(300));
            Behaviors.Add(new CollectBehavior<Twig>(new Twig(), 2, _twigCount));

        }

        public override string Name => Resource.Branch;
        /*    
            public int GetSmthPerCollectCount()
            {
                return 2;
            }

            public int GetSmthTotalCount()
            {
                return _twigCount;
            }

            public void SetSmthTotalCount(int totalCount)
            {
                _twigCount = totalCount;
            }

            public Twig GetSmth()
            {
                return new Twig();
            }*/

        public override GameObject Clone()
        {
            return new Branch();
        }
    }
}
