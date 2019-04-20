using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Tools;
using Unity;

namespace Engine.Actions
{
    class MakeFireWithStone: IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name => "Make fire with stone";

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
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
                return FinishedActionResult.Instance;

            return new ConseqActionResult(true, 
                Consequance.ProbabilityOrElse(
                    hero.GetKnowledge(Knowledges.MakeFireWithStone), 
                    Consequance.Composite(
                        Create(branch, plant),
                        Consequance.AddKnowledge(Knowledges.MakeFireWithStone, 10),
                        Consequance.AddObjectKnowledge(plant.Name, 1)),
                    Consequance.Composite(Consequance.AddKnowledge(Knowledges.MakeFireWithStone, 5), Consequance.AddObjectKnowledge(plant.Name, 1)))
                    ); 
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero, Point actionPosition)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct().ToList();

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

        public Action<Hero> Create(GameObject branch, GameObject plant)
        {
            return hero =>
            {
                branch.RemoveFromContainer();
                plant.RemoveFromContainer();
                var fire = new Fire();

                Map.SetHObjectFromDestination(hero.Position, fire);
            };
        }
    }
}
