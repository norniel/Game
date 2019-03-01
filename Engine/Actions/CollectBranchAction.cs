using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;

namespace Engine.Actions
{
    class CollectBranchAction: CollectSmth<Branch>
    {
        public override string Name => "Collect branch";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectBranch;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectBranch) && obj.GetBehavior<CollectBehavior<Branch>>()?.CurrentCount > 0);
        }

        public override double GetTiredness()
        {
            return 0.5;
        }
    }
}
