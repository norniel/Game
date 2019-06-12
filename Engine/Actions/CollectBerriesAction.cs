using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;

namespace Engine.Actions
{
    internal class CollectBerriesAction:CollectSmth<Berry>
    {
        public override string Name => "Collect berries";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectBerries;
        }

        public override double GetTiredness()
        {
            return 0.1;
        }
    }
}
