using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Microsoft.Practices.Unity;

namespace Engine.Actions
{
    class MakeFire: IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name {
            get { return ActionsResource.MakeFire; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToCreateFire;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var branches = objects.Where(o => o is Branch).ToList();
            var plant = objects.SingleOrDefault(o => o is Plant);

            if (branches.Count != 2 || plant == null)
                return true;

            branches.ForEach(b => b.RemoveFromContainer());
            plant.RemoveFromContainer();
            var fire = new Fire();

            Map.SetObjectFromDestination(hero.Position, fire as FixedObject);

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct();

            var branches = allObjects.Where(ao => ao is Branch).Select(ao => ao).Take(2).ToList();
            var plant = allObjects.FirstOrDefault(ao => ao is Plant);

            if (branches.Count == 2 && plant != null)
            {
                branches.Add(plant);
                yield return branches.ToList();
            }
        }

        public double GetTiredness()
        {
            return 10;
        }
    }
}
