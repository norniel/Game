using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class TreeContext : IObjectContext
    {
        public uint EmptyId { get; set; } = 0x00000300;
        public uint HalfEmptyId { get; set; } = 0x00000200;
        public uint Id { get; set; } = 0x00000100;

        public string Name { get; set; } = "Tree";
        public string BaseName { get; set; } = "Tree";
        public int Weight { get; set; } = 1;
        public bool NeedKnowledge { get; set; }
        public uint BaseId { get; set; }

        public int BerriesCount { get; set; } = 4;

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Cuttable,
            Property.CollectBerries,
            Property.CollectBranch,
            Property.CollectTwig
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>
        {
            new CollectBehavior<Berry>("Berry", 2, 4),
            new CollectBehavior<Branch>("Branch", 1, 4),
            new CollectBehavior<Twig>("Twig", 2, 16)
        };

        public GameObject Produce()
        {
            return new Tree(this);
        }
    }

    class Tree : FixedObject
    {
        private readonly TreeContext _treeContext;
        public Tree(TreeContext treeContext) :base(treeContext)
        {
            _treeContext = treeContext;
            IsPassable = false;
            Height = 2;
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
            Behaviors.Add(new CollectBehavior<Berry>("Berry", 2, 4));
            Behaviors.Add(new CollectBehavior<Branch>("Branch", 1, 4));
            Behaviors.Add(new CollectBehavior<Twig>("Twig", 2, 16));
        }

        public override uint GetDrawingCode()
        {
            var berryCollect = GetBehavior<CollectBehavior<Berry>>();
            if (berryCollect == null)
                return Id;

            var berriesCount = berryCollect.CurrentCount;

            if (berriesCount > _treeContext.BerriesCount / 2) return Id;

            if (berriesCount <= _treeContext.BerriesCount / 2 && berriesCount > 0) return _treeContext.HalfEmptyId;

            return _treeContext.EmptyId;
        }

        public virtual int Hardness { get; set; }
    }
}
