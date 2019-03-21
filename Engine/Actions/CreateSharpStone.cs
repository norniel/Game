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

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
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

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var stones = objects.Where(o => o is Rock).Take(2).ToList();

            if (stones.Count < 2)
                return FinishedActionResult.Instance;



            return new ConseqActionResult(true, CreateIfKnowledge(stones.First()));
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
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

        public static Action<Hero> CreateIfKnowledge(GameObject stone)
        {
            return hero =>
            {
                stone.RemoveFromContainer();

                Consequance.ProbabilityOrElse(
                    hero.GetKnowledge(Knowledges.CreateSharpStone),
                    Consequance.Composite(
                        heroInner =>
                        {
                            var sharpStone = new SharpStone();
                            Game.AddToGame(heroInner, sharpStone);
                        },
                        Consequance.AddKnowledge(Knowledges.CreateSharpStone, 10)),
                    Consequance.AddKnowledge(Knowledges.CreateSharpStone, 5))(hero);                
            };
        }
    }
}
