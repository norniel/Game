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

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
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

            objectWithSmth.SetSmthTotalCount(objectWithSmth.GetSmthTotalCount() < objectWithSmth.GetSmthPerCollectCount()
                ? 0
                : objectWithSmth.GetSmthTotalCount() - objectWithSmth.GetSmthPerCollectCount());

            var smthtoBag = new List<T>();

            for (int i = 0; i < smthToBagCount; i++)
            {
                smthtoBag.Add(objectWithSmth.GetSmth());
            }
            hero.AddToBag(smthtoBag);

            return objectWithSmth.GetSmthTotalCount() > 0;
        }
    }
}
