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
    public class CreateDiggingStickAction : IAction
    {
        public string Name => ActionsResource.CreateDiggingStick;

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return Property.Cutter == property || Property.Branch == property;
        }

        public bool Do(Hero hero, IList<GameObject> objects)
        {
            var gameObjects = objects as GameObject[] ?? objects.ToArray();
            var branch = gameObjects.SingleOrDefault(o => o is Branch);
            var stone = gameObjects.SingleOrDefault(ao => ao.Properties.Contains(Property.Cutter));

            if (branch == null || stone == null)
                return true;

            branch.RemoveFromContainer();
            var diggingStick = new DiggingStick();

            if (!hero.AddToBag(diggingStick))
            {
                Game.Map.SetHObjectFromDestination(hero.Position, diggingStick);
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct();

            var gameObjects = allObjects as GameObject[] ?? allObjects.ToArray();

            var branch = gameObjects.FirstOrDefault(ao => ao is Branch);
            var stone = gameObjects.FirstOrDefault(ao => ao.Properties.Contains(Property.Cutter));

            if (branch != null && stone != null)
            {
                yield return new List<GameObject> { branch, stone };
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
