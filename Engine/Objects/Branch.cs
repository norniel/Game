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

        public override int Weight { get { return 5; } }

        public override void InitializeProperties()
        {
            this.Properties = new HashSet<Property>
            {
               Property.Pickable,
               Property.NeedToCreateStoneAxe,
               Property.NeedToCreateFire,
               Property.CollectTwig
            };
        }

        public override string Name
        {
            get { return Resource.Branch; }
        }

        public int TimeOfBurning {
            get { return 300; }
        }

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
