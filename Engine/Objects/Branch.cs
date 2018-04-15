using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Resources;

namespace Engine.Objects
{
    class Branch : FixedObject
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

        public override GameObject Clone()
        {
            return new Branch();
        }
    }
}
