using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;

namespace Engine.Actions
{
    internal class EatAction : IAction
    {
        public string Name {
            get { return "Eat"; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Eatable;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            foreach (var removableObject in objects.OfType<IEatable>())
            {
                hero.Eat(removableObject.Satiety);
                (removableObject as GameObject).RemoveFromContainer();
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Eatable));
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Eatable)).ToList();
        }

        public double GetTiredness()
        {
            return 0;
        }
    }
}
