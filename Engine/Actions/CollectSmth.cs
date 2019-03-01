using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    internal abstract class CollectSmth<T> : IAction where T : GameObject
    {
        public abstract string Name { get; }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public virtual string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            var objectToCollectFrom = objects
                .Select(x => x.GetBehavior<CollectBehavior<T>>()).FirstOrDefault();

            if (objectToCollectFrom == null)
            {
                return Name;
            }

            return string.Format(ActionsResource.Collect, objectToCollectFrom.Name);
        }

        public abstract bool IsApplicable(Property property);

        public virtual IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var collectBehavior = objects.Select(x => x.GetBehavior<CollectBehavior<T>>())
                .FirstOrDefault();

            var actionIsNotOver = Collect(collectBehavior, hero);

            return new ConseqActionResult(!actionIsNotOver, Consequance.Probability(0.5, Consequance.AddObjectKnowledge(collectBehavior?.Name, 1)));
        }

        public abstract bool CanDo(Hero hero, IEnumerable<GameObject> objects);

        public virtual IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero)
        {
            var necessaryObjects = objects.Where(obj => obj.Properties.Any(IsApplicable))
                .Where(x => x.HasBehavior<CollectBehavior<T>>()) //.OfType<IHasSmthToCollect<T>>()
                .Where(x => x.GetBehavior<CollectBehavior<T>>().CurrentCount >
                            0) //.Where(x => x.GetSmthTotalCount() > 0)
                //.Cast<GameObject>()
                .ToList();

            if (necessaryObjects.Any())
                yield return necessaryObjects;
        }

        public abstract double GetTiredness();

        private bool Collect(CollectBehavior<T> objectWithSmth, Hero hero)
        {
            if (objectWithSmth.CurrentCount <= 0)
                return false;

            int smthToBagCount = objectWithSmth.CurrentCount < objectWithSmth.PerCollectCount
                ? objectWithSmth.CurrentCount
                : objectWithSmth.PerCollectCount;

            var addedToBagCount = 0;

            for (int i = 0; i < smthToBagCount; i++)
            {
                var objToBag = objectWithSmth.GetSmth();
                if (!hero.AddToBag(objToBag))
                    break;
                
                addedToBagCount++;
            }

            var newCurrent = objectWithSmth.CurrentCount - addedToBagCount;

            objectWithSmth.CurrentCount = Math.Max(0, newCurrent);;

            return objectWithSmth.CurrentCount > 0 && addedToBagCount == smthToBagCount && !hero.Bag.IsFull;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}