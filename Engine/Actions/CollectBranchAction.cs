using Engine.Heros;
using Engine.Interfaces;

namespace Engine.Actions
{
    using System.Collections.Generic;
    using System.Linq;
    using Objects;
    class CollectBranchAction: CollectSmth<Branch>
    {
        public override string Name
        {
            get { return "Collect branch"; }
        }

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectBranch;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectBranch) && ((IHasSmthToCollect<Branch>)obj).GetSmthTotalCount() > 0);
        }

        public override double GetTiredness()
        {
            return 0.5;
        }
    }
}
