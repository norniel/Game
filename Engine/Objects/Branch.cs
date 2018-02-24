using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    class Branch : FixedObject, IBurnable, IHasSmthToCollect<Twig>
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

        public override string Name => Resource.Branch;

        public int TimeOfBurning => 300;

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
        }
    }
}
