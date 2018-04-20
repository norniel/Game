﻿using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    public class BranchContext : IObjectContext
    {
        public uint Id { get; set; } = 0x00000800;

        public string Name { get; set; } = Resource.Branch;

        public HashSet<Property> Properties { get; set; } = new HashSet<Property>
        {
            Property.Pickable,
            Property.NeedToCreateStoneAxe,
            Property.NeedToMakeFireWithWood,
            Property.CollectTwig,
            Property.Branch,
            Property.NeedToBuildWickiup
        };

        public HashSet<IBehavior> Behaviors { get; set; } = new HashSet<IBehavior>
        {
            new BurnableBehavior(300),
            new CollectBehavior<Twig>("Twig", 2, 4)
        };

        public int Weight { get; set; } = 5;

        public GameObject Produce()
        {
            return new Branch(this);
        }
    }

    class Branch : FixedObject
    {
        private readonly BranchContext _context;
        public Branch(BranchContext context): base(context)
        {
            _context = context;
        }
    }
}
