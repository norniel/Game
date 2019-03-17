using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;
using JetBrains.Annotations;

namespace Engine.Actions
{
    [UsedImplicitly]
    internal class BurnAction : IAction
    {
        public string Name => "Burn";

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            var burnable = objects.FirstOrDefault(o => o.HasBehavior<BurnableBehavior>());

            if (burnable == null)
            {
                return Name;
            }

            var name = hero.IsBaseToShow(burnable) ? burnable.GetBaseName() : burnable.Name;

            return string.Format(ActionsResource.AddToFire, name);
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Burning;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var gameObjects = objects as GameObject[] ?? objects.ToArray();
            var burnable = gameObjects.FirstOrDefault(o => o.HasBehavior< BurnableBehavior>());
            var burning = gameObjects.OfType<IBurning>().FirstOrDefault();

            if (burnable == null || burning == null)
            {
                return new FinishedActionResult();
            }

            var burnableBehavior = burnable.GetBehavior<BurnableBehavior>();
            burning.TimeOfBurning += (int)(burnableBehavior.Сoefficient * burnable.WeightDbl);
            burnable.RemoveFromContainer();

            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var burningObjects = hero.GetContainerItems().Where(o => o.HasBehavior<BurnableBehavior>())
                .GroupBy(o => o.GetType()).Select(gr => gr.First());

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
