using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    internal class DropAction : IAction
    {
        public string Name {
            get { return "Drop"; }
        }
        public bool IsApplicable(Property property)
        {
            return property == Property.Dropable;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            hero.AddToBag(objects.Select(o => o.GameObject));

            foreach (var removableObject in objects)
            {
                removableObject.RemoveFromContainer();
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Dropable));
        }
    }
}
