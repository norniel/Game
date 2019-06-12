using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    internal class PickAction : IAction
    {
        public string Name => ActionsResource.Pick;

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            hero.AddToBag(objects);

            var conseqList = objects.Select(o => Consequance.AddObjectKnowledge(o.Name, 1)).ToArray();

            return new ConseqActionResult(true, conseqList);
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero, Point actionPosition)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Pickable)).ToList();// && hero.HasEmpptyContainer;
        }

        public double GetTiredness()
        {
            return 0.5;
        }

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Pickable;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
