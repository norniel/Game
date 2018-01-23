using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    internal class BurnAction : IAction
    {
        public string Name => "Burn";

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            var burnable = objects.FirstOrDefault(o => o is IBurnable);

            if (burnable == null)
            {
                return Name;
            }

            return string.Format(ActionsResource.AddToFire, burnable.Name);
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Burning;
        }

        public IActionResult Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var burnable = objects.FirstOrDefault(o => o is IBurnable);
            var burning = objects.OfType<IBurning>().FirstOrDefault();

            if (burnable == null || burning == null)
            {
                return new FinishedActionResult();
            }

            burning.TimeOfBurning += ((IBurnable)burnable).TimeOfBurning;
            burnable.RemoveFromContainer();

            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var burningObjects = hero.GetContainerItems().Where(o => o is IBurnable).GroupBy(o => o.GetType()).Select(gr => gr.First());

            return burningObjects.Select(bo => new List<GameObject> {bo}.Union(objects).ToList());
        }

        public double GetTiredness()
        {
            return 0.2;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
