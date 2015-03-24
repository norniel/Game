using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine.Actions
{
    class CollectTwig : CollectSmth<Twig>
    {
        public override string Name
        {
            get { return "Collect twig"; }
        }

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectTwig;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectTwig) && ((IHasSmthToCollect<Twig>)obj).GetSmthTotalCount() > 0);
        }
    }
}
