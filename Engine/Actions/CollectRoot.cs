using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Actions
{
    class CollectRoot : CollectSmth<Root>
    {
        public override string Name => "Collect root";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectRoot;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectRoot) && ((IHasSmthToCollect<Root>)obj).GetSmthTotalCount() > 0);
        }

        public override double GetTiredness()
        {
            return 1;
        }

        public override bool Do(Hero hero, IList<GameObject> objects)
        {
            var digger = objects.FirstOrDefault(d => d.Properties.Contains(Property.Digger));

            if (digger == null)
                return true;

            return base.Do(hero, objects);
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero)
        {
            var result = base.GetActionsWithNecessaryObjects(objects, hero);

            var digger = hero.GetContainerItems().FirstOrDefault(d => d.Properties.Contains(Property.Digger));

            if (digger == null)
                yield break;
            foreach (var gameObjects in result)
            {
                gameObjects.Add(digger);

                yield return gameObjects;
            }
        }
    }
}
