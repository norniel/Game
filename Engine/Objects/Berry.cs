using System.Collections.Generic;
using Engine.Behaviors;
using Engine.Interfaces;
using Engine.Resources;

namespace Engine.Objects
{
    public class BerryContext : IObjectContext
    {
        public uint Id { get; set; } = 0x00000700;

        public string Name { get; set; } = "Berry";

        public int Weight { get; set; } = 1;

        public HashSet<Property> Properties { get; set; } = new HashSet<Property>
        {
            Property.Pickable,
            Property.Eatable
        };

        public HashSet<IBehavior> Behaviors { get; set; } = new HashSet<IBehavior>{};

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
