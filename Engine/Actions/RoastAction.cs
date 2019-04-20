using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;
using Engine.Resources;

namespace Engine.Actions
{
    internal class RoastAction : LongActionBase
    {
        public override string Name => "Roast";

        public override string GetName(IEnumerable<GameObject> objects,Hero hero)
        {
            var gameObject = objects.First();
            var name = hero.IsBaseToShow(gameObject) ? gameObject.GetBaseName() : gameObject.Name;

            return string.Format(ActionsResource.Roast, name);
        }

        public override bool IsApplicable(Property property)
        {
            return property == Property.Burning;
        }

        protected override bool DoNotLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var hasBurning = objects.Any(o => o is IBurning);
            var hasTwig = objects.Any(o => o is Twig);
            var hasRoastable = objects.Any(o => o is Twig);

            if (!hasBurning || !hasTwig || !hasRoastable)
            {
                return true;
            }

            return false;
        }

        protected override void DoLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var hasBurning = objects.Any(o => o is IBurning);
            var twig = objects.OfType<Twig>().FirstOrDefault();
            var roastable = objects.Where(o => o.HasBehavior<RoastBehavior>()).ToList();

            if (!hasBurning || twig == null || !roastable.Any())
            {
                return;
            }

            twig.RemoveFromContainer();
            foreach (var r in roastable)
            {
                if(!(r.GetBehavior<RoastBehavior>() is RoastBehavior roastBehavior))
                    continue;

                var roasted = roastBehavior.GetRoasted();

                if (!hero.AddToBag(roasted))
                    break;

                r.RemoveFromContainer();

            }
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero, Point actionPosition)
        {
            var roastingObjects = hero.GetContainerItems()
                .Where(o => o.Properties.Contains(Property.Roastable) && o.HasBehavior<RoastBehavior>())
                .GroupBy(o => o.GetType())
                .Select(gr => gr.Take(3));

            var twig = hero.GetContainerItems().OfType<Twig>().FirstOrDefault();

            if (twig != null)
                return roastingObjects.Select(bo => bo.Union(new List<GameObject> {twig}).Union(objects).ToList());

            return new List<List<GameObject>>();
        }

        public override double GetTiredness()
        {
            return 0.1;
        }

        protected override int ElapsedActionTime{ get; set; }

        protected override int TotalActionTime => 2;
    }
}
