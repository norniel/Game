using System;
using System.Collections.Generic;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine
{
    public class FixedObjectContext : IObjectContext
    {
        public virtual GameObject Produce()
        {
            return new FixedObject(this);
        }

        public Func<HashSet<Property>> Properties { get; set; }
        public Func<HashSet<IBehavior>> Behaviors { get; set; }

        public int Weight { get; set; } = 1;
        public bool NeedKnowledge { get; set; }
        public uint BaseId { get; set; }
        public string BaseName { get; set; }

        public virtual string Name { get; set; }

        public uint Id { get; set; }
    }
}
