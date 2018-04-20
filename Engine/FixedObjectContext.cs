using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class FixedObjectContext : IObjectContext
    {
        public GameObject Produce()
        {
            return new FixedObject(this);
        }

        public HashSet<Property> Properties { get; set; }
        public HashSet<IBehavior> Behaviors { get; set; }

        public int Weight { get; set; } = 1;

        public string Name { get; set; }

        public uint Id { get; set; }
    }
}
