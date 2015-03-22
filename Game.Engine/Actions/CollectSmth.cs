using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    abstract class CollectSmth<T> : IAction where T: GameObject
    {
        public abstract string Name { get; }

        public virtual string GetName(IEnumerable<GameObject> objects)
        {
            var objectToCollectFrom = objects.OfType<IHasSmthToCollect<T>>().FirstOrDefault();

            if (objectToCollectFrom == null)
            {
                return Name;
            }

            return string.Format("Collect {0}", objectToCollectFrom.GetSmth().Name);
        }

        public abstract bool IsApplicable(Property property);

        public virtual bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var actionIsNotOver = objects.OfType<IHasSmthToCollect<T>>().Any(hb => this.Collect(hb, hero));

            return !actionIsNotOver;
        }

        public abstract bool CanDo(Hero hero, IEnumerable<GameObject> objects);

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
           yield return objects.Where(obj => obj.Properties.Any(this.IsApplicable)
               && obj is IHasSmthToCollect<T>
               && (obj as IHasSmthToCollect<T>).GetSmthTotalCount() > 0).ToList();
        }

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
    }
}
