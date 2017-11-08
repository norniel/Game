using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Wrapers;
using Microsoft.Practices.Unity;

namespace Engine.Actions
{
    internal class DropAction : IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name => ActionsResource.Drop;

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Dropable;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            foreach (var removableObject in objects)
            {
                Map.SetHObjectFromDestination(hero.Position, removableObject as FixedObject);
              //  removableObject.RemoveFromContainer();
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Dropable));
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Dropable)).ToList();
        }

        public double GetTiredness()
        {
            return 0.1;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
