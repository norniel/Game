using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Tools;
using Unity.Attributes;

namespace Engine.Actions
{
    class MakeFireWithStone: IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name => "Make fire with stone";

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToMakeFireWithStone;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.MakeFireWithStone;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var branch = objects.SingleOrDefault(o => o is Branch);
            var plant = objects.SingleOrDefault(o => o is Plant);
            var stones = objects.Where(o => o.Properties.Contains(Property.Stone)).ToList();


            if (stones.Count < 2 || plant == null || branch == null)
                return new FinishedActionResult();

            branch.RemoveFromContainer();
            plant.RemoveFromContainer();
            var fire = new Fire();

            Map.SetHObjectFromDestination(hero.Position, fire);

            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct();

            var stones = allObjects.Where(o => o.Properties.Contains(Property.Stone)).Select(ao => ao).Take(2).ToList();
            var branch = allObjects.FirstOrDefault(ao => ao is Branch);
            var plant = allObjects.FirstOrDefault(ao => ao is Plant);

            if (stones.Count == 2 && plant != null && branch != null)
            {
                stones.Add(plant);
                stones.Add(branch);
                yield return stones.ToList();
            }
        }

        public double GetTiredness()
        {
            return 5;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
