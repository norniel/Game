using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    public class CreateStoneAxeAction:IAction
    {
        public string Name {
            get { return "Create Stone axe"; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return 
                Property.NeedToCreateStoneAxe == property;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var branch = objects.SingleOrDefault(o => o is Branch);
            var stone = objects.SingleOrDefault(o => o is Rock);

            if (branch == null || stone == null)
                return true;

            branch.RemoveFromContainer();
            stone.RemoveFromContainer();
            var axe = new StoneAxe();

            if (!hero.AddToBag(axe))
            {
                Game.Map.SetObjectFromDestination(hero.Position, axe);
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.OfType<Branch>().Any();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).Distinct();

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
    }
}
