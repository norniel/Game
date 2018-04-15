using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Actions
{
    class CollectTwig : CollectSmth<Twig>
    {
        public override string Name => "Collect twig";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectTwig;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectTwig) && (obj.GetBehavior(typeof(CollectBehavior<Twig>)) as CollectBehavior<Twig>)?.CurrentCount > 0);
        }

        public override double GetTiredness()
        {
            return 0.1;
        }
    }
}
