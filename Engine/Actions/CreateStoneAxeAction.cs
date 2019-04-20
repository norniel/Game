﻿using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    public class CreateStoneAxeAction:IAction
    {
        public string Name => ActionsResource.CreateStoneAxe;

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return 
                Property.NeedToCreateStoneAxe == property;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var branch = objects.SingleOrDefault(o => o is Branch);
            var stone = objects.SingleOrDefault(o => o is Rock);

            if (branch == null || stone == null)
                return FinishedActionResult.Instance;

            branch.RemoveFromContainer();
            stone.RemoveFromContainer();
            var axe = new StoneAxe();

            Game.AddToGame(hero, axe);

            return FinishedActionResult.Instance;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.OfType<Branch>().Any();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero, Point actionPosition)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct().ToList();

            var branch = allObjects.FirstOrDefault(ao => ao is Branch);
            var stone = allObjects.FirstOrDefault(ao => ao is Rock);

            if (branch != null && stone != null)
            {
                yield return new List<GameObject> { branch, stone };
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
