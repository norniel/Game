using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine
{
    public class CutAction : IAction
    {
        public string Name {
            get { return "Cut"; }
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Cuttable;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(o => o.Properties.Contains(Property.Cuttable));
        }
    }
}
