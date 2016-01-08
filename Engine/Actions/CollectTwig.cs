﻿using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Actions
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

        public override double GetTiredness()
        {
            return 0.1;
        }
    }
}