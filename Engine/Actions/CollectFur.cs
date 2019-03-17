using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;
using Engine.Tools;

namespace Engine.Actions
{
    class CollectFur:CollectSmth<Fur>
    {
        public override string Name => "Collect fur";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectFur;
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectFur) && obj.GetBehavior<CollectBehavior<Meat>>()?.CurrentCount > 0);
        }

        public override double GetTiredness()
        {
            return 1;
        }

        public override IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var cutter = objects.FirstOrDefault(d => d.Properties.Contains(Property.Cutter));

            if (cutter == null)
                return new FinishedActionResult();

            return base.Do(hero, objects);
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero)
        {
            var result = base.GetActionsWithNecessaryObjects(objects, hero);

            var cutter = hero.GetContainerItems().FirstOrDefault(d => d.Properties.Contains(Property.Cutter));

            if (cutter == null)
                yield break;

            foreach (var gameObjects in result)
            {
                gameObjects.Add(cutter);

                yield return gameObjects;
            }
        }
    }
}
