using System.Collections.Generic;
using Engine.Objects;

namespace Engine.Interfaces
{
    public interface IObjectContext
    {
        GameObject Produce();
        HashSet<Property> Properties { get; set; }
        HashSet<IBehavior> Behaviors { get; set; }

        uint Id { get; set; }
        string Name { get; set; }
        int Weight { get; set; }
    }
}
