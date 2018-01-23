using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Tool;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    public class CreateSharpStone:IAction
    {
        public string Name => ActionsResource.CreateSharpStone;

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.CreateSharpStone;
        }

        public bool IsApplicable(Property property)
        {
            return Property.Stone == property;
        }

        public IActionResult Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var stones = objects.Where(o => o is Rock).Take(2).ToList();

            if (stones.Count < 2)
                return new FinishedActionResult();

            return new ConseqActionResult(true, CreateSharpStone.Create(stones.First()));
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects = objects.Union(hero.GetContainerItems()).Distinct();

            var stones = allObjects.Where(o => o is Rock).Take(2).ToList();

            if (stones.Count == 2)
            {
                yield return stones;
            }
        }

        public double GetTiredness()
        {
            return 2;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }

        public static Action<Hero> Create(GameObject stone)
        {
            return hero =>
            {
                stone.RemoveFromContainer();
                var sharpStone = new SharpStone();
                Game.AddToGame(hero, sharpStone);
            };
        }
    }
}
