using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;
using Microsoft.Practices.Unity;

namespace Game.Engine.Actions
{
    internal class DropAction : IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name {
            get { return "Drop"; }
        }

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
                Map.SetObjectFromDestination(hero.Position, removableObject as FixedObject);
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
    }
}
