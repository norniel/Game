using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Objects;
using Engine.Tools;

namespace Engine.Actions
{
    class CollectMeat :CollectSmth<Meat>
    {
        public override string Name => "Collect meat";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectMeat;
        }

        public override double GetTiredness()
        {
            return 1;
        }

        public override IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var cutter = objects.FirstOrDefault(d => d.Properties.Contains(Property.Cutter));

            if (cutter == null)
                return FinishedActionResult.Instance;

            return base.Do(hero, objects);
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero, Point actionPosition)
        {
            var result = base.GetActionsWithNecessaryObjects(objects, hero, actionPosition);

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
