using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    internal class EatAction : IAction
    {
        public string Name {
            get { return "Eat"; }
        }
        public bool IsApplicable(Property property)
        {
            return property == Property.Eatable;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            foreach (var removableObject in objects)
            {
                hero.Eat();
                removableObject.RemoveFromContainer();
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Eatable));
        }
    }
}
