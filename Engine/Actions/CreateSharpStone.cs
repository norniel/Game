using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Tool;
using Engine.Resources;

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

        public bool Do(Hero hero, IList<GameObject> objects)
        {
            var stones = objects.Where(o => o is Rock).Take(2).ToList();

            if (stones.Count < 2)
                return true;

            stones.First().RemoveFromContainer();
            var sharpStone = new SharpStone();

            if (!hero.AddToBag(sharpStone))
            {
                Game.Map.SetHObjectFromDestination(hero.Position, sharpStone);
            }

            return true;
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
    }
}
