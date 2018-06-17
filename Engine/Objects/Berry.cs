using System;
using System.Collections.Generic;
using Engine.Interfaces;

namespace Engine.Objects
{
    public class BerryContext : IObjectContext
    {
        public uint Id { get; set; } = 0x00000700;

        public string Name { get; set; } = "Berry";

        public int Weight { get; set; } = 1;
        public bool NeedKnowledge { get; set; }
        public uint BaseId { get; set; }

        public Func<HashSet<Property>> Properties { get; set; } = () => new HashSet<Property>
        {
            Property.Pickable,
            Property.Eatable
        };

        public Func<HashSet<IBehavior>> Behaviors { get; set; } = () => new HashSet<IBehavior>{};

        public GameObject Produce()
        {
            return new Berry(this);
        }
    }

    public class Berry : FixedObject
    {
        private readonly BerryContext _berryContext;
        public Berry(BerryContext berryContext):base(berryContext)
        {
            _berryContext = berryContext;
        }
    }
}
