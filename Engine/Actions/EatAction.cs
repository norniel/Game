using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    internal class EatAction : IAction
    {
        public string Name => ActionsResource.Eat;

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Eatable;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            foreach (var eatableObject in objects.Where(o => o.HasBehavior(typeof(EatableBehavior))))
            {
                var eatableBehavior = eatableObject.GetBehavior(typeof(EatableBehavior)) as EatableBehavior;
                hero.Eat((int)(eatableBehavior.SatietyCoefficient * eatableObject.WeightDbl));
                eatableObject.RemoveFromContainer();
            }

            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Eatable));
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Eatable)).ToList();
        }

        public double GetTiredness()
        {
            return 0;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
