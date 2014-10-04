using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;
using Microsoft.Practices.Unity;

namespace Game.Engine.Actions
{
    class MakeFire: IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name {
            get { return "Make Fire"; }
        }
        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToCreateFire;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            var branches = objects.Where(o => o.GameObject is Branch).ToList();
            var plant = objects.SingleOrDefault(o => o.GameObject is Plant);

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

        public IEnumerable<List<RemovableWrapper<GameObject>>> GetActionsWithNecessaryObjects(IEnumerable<RemovableWrapper<GameObject>> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItemsAsRemovable()).Distinct(new RemovableObjecctsComparer<GameObject>());

            var branches = allObjects.Where(ao => ao.GameObject is Branch).Select(ao => ao).Take(2).ToList();
            var plant = allObjects.FirstOrDefault(ao => ao.GameObject is Plant);

            if (branches.Count == 2 && plant != null)
            {
                branches.Add(plant);
                yield return branches.ToList();
            }
        }
    }
}
