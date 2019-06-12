using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;
using Engine.Tools;

namespace Engine.Actions
{
    class CollectRoot : CollectSmth<Root>
    {
        public override string Name => "Collect root";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectRoot;
        }

        public override double GetTiredness()
        {
            return 1;
        }

        public override IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var digger = objects.FirstOrDefault(d => d.Properties.Contains(Property.Digger));

            if (digger == null)
                return FinishedActionResult.Instance;

            return base.Do(hero, objects);
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero, Point actionPosition)
        {
            var result = base.GetActionsWithNecessaryObjects(objects, hero, actionPosition);

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
