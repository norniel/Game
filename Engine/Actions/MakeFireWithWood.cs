using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;
using Unity.Attributes;

namespace Engine.Actions
{
    class MakeFireWithWood: IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name => ActionsResource.MakeFire;

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToMakeFireWithWood;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var branches = objects.Where(o => o is Branch).ToList();
            var plant = objects.SingleOrDefault(o => o is Plant);

            if (branches.Count != 2 || plant == null)
                return new FinishedActionResult();

            branches.ForEach(b => b.RemoveFromContainer());
            plant.RemoveFromContainer();
            var fire = new Fire();

            Map.SetHObjectFromDestination(hero.Position, fire);

            return new ConseqActionResult(true, Consequance.Probability(0.5, Consequance.AddObjectKnowledge(plant.Name, 1)));
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
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

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
