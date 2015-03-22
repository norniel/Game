using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

namespace Game.Engine.Actions
{
    internal class RoastAction : IAction
    {
        public string Name
        {
            get { return "Roast"; }
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return string.Format("Roast {0}", objects.First().Name);
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Burning;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var burnable = objects.FirstOrDefault(o => o is IBurnable);
            var branch = objects.OfType<Branch>().FirstOrDefault();
            var roastable = objects.OfType<IRoastable>().ToList();

            if (burnable == null || branch == null || !roastable.Any())
            {
                return true;
            }

            branch.RemoveFromContainer();
            foreach (var r in roastable)
            {
                var roasted = r.GetRoasted();

                if (!hero.AddToBag(roasted))
                    break;

                ((GameObject) r).RemoveFromContainer();
            }

            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var roastingObjects = hero.GetContainerItems()
                .Where(o => o.Properties.Contains(Property.Roastable) && o is IRoastable)
                .GroupBy(o => o.GetType())
                .Select(gr => gr.First());

            var branch = hero.GetContainerItems().OfType<Branch>().FirstOrDefault();

            if (branch != null)
                return roastingObjects.Select(bo => new List<GameObject> { bo, branch }.Union(objects).ToList());

            return new List<List<GameObject>>();
        }
    }
}
