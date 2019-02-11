using System;
using System.Collections.Generic;
using Engine.Objects;

namespace Engine.Interfaces
{
    public interface IObjectContext
    {
        GameObject Produce();
        Func<HashSet<Property>> Properties { get; set; }
        Func<HashSet<IBehavior>> Behaviors { get; set; }

        uint Id { get; set; }
        string Name { get; set; }
        int Weight { get; set; }

        bool NeedKnowledge { get; set; }
        uint BaseId { get; set; }
        string BaseName { get; set; }
    }
}
