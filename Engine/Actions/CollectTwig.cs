using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;
using JetBrains.Annotations;

namespace Engine.Actions
{
    [UsedImplicitly]
    class CollectTwig : CollectSmth<Twig>
    {
        public override string Name => "Collect twig";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectTwig;
        }

        public override double GetTiredness()
        {
            return 0.1;
        }
    }
}
