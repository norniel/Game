using System;
using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Objects
{
    public class BushContext : IObjectContext
    {
        public GameObject Produce()
        {
            return new Bush(this);
        }

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Cuttable,
            Property.CollectBerries,
            Property.CollectBranch
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>()
        {
            new CollectBehavior<Berry>("RaspBerries", 2, 2),
            new CollectBehavior<Branch>("Branch", 2, 4),
        };

        public uint Id { get; set; } = 0x00001200;
        public string Name { get; set; } = Resource.Bush;
        public int Weight { get; set; } = 1;
    }

    [GenerateMap]
    class Bush : FixedObject
    {
        public Bush(BushContext bushContext):base(bushContext)
        {
            IsPassable = false;
        }
        
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
            Behaviors.Add(new CollectBehavior<Berry>("RaspBerries", 2, 2));
            Behaviors.Add(new CollectBehavior<Branch>("Branch", 2, 4));
        }
    }
}
