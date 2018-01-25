using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;

namespace Engine.Actions
{
    internal abstract class CollectSmth<T> : IAction where T: GameObject
    {
        public abstract string Name { get; }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public virtual string GetName(IEnumerable<GameObject> objects)
        {
            var objectToCollectFrom = objects.OfType<IHasSmthToCollect<T>>().FirstOrDefault();

            if (objectToCollectFrom == null)
            {
                return Name;
            }

            return string.Format(ActionsResource.Collect, objectToCollectFrom.GetSmth().Name);
        }

        public abstract bool IsApplicable(Property property);

        public virtual bool Do(Hero hero, IList<GameObject> objects)
        {
            var actionIsNotOver = objects.OfType<IHasSmthToCollect<T>>().Any(hb => Collect(hb, hero));

            return !actionIsNotOver;
        }

        public abstract bool CanDo(Hero hero, IEnumerable<GameObject> objects);

        public virtual IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero)
        {
            var necessaryObjects = objects.Where(obj => obj.Properties.Any(IsApplicable))
                .OfType<IHasSmthToCollect<T>>()
                .Where(x => x.GetSmthTotalCount() > 0)
                .Cast<GameObject>()
                .ToList();

            if(necessaryObjects.Any())
                yield return necessaryObjects;
        }

        public abstract double GetTiredness();

        private bool Collect(IHasSmthToCollect<T> objectWithSmth, Hero hero)
        {
            if (objectWithSmth.GetSmthTotalCount() <= 0)
                return false;

            int smthToBagCount = objectWithSmth.GetSmthTotalCount() < objectWithSmth.GetSmthPerCollectCount() ?
                objectWithSmth.GetSmthTotalCount() :
                objectWithSmth.GetSmthPerCollectCount();

            var addedToBagCount = 0;

            for (int i = 0; i < smthToBagCount; i++)
            {
                var objToBag = objectWithSmth.GetSmth();
                if(!hero.AddToBag(objToBag))
                    break;

                addedToBagCount++;
            }

            objectWithSmth.SetSmthTotalCount(objectWithSmth.GetSmthTotalCount() < addedToBagCount
                ? 0
                : objectWithSmth.GetSmthTotalCount() - addedToBagCount);

            return objectWithSmth.GetSmthTotalCount() > 0 && addedToBagCount == smthToBagCount && !hero.Bag.IsFull;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
