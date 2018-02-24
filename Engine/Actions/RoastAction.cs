using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;
using Engine.Resources;

namespace Engine.Actions
{
    internal class RoastAction : LongActionBase
    {
        public override string Name => "Roast";

        public override string GetName(IEnumerable<GameObject> objects)
        {
            return string.Format(ActionsResource.Roast, objects.First().Name);
        }

        public override bool IsApplicable(Property property)
        {
            return property == Property.Burning;
        }

        protected override bool DoNotLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var burnable = objects.FirstOrDefault(o => o is IBurnable);
            var twig = objects.OfType<Twig>().FirstOrDefault();
            var roastable = objects.OfType<IRoastable>().ToList();

            if (burnable == null || twig == null || !roastable.Any())
            {
                return true;
            }

            return false;
        }

        protected override void DoLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var burnable = objects.FirstOrDefault(o => o is IBurnable);
            var twig = objects.OfType<Twig>().FirstOrDefault();
            var roastable = objects.OfType<IRoastable>().ToList();

            if (burnable == null || twig == null || !roastable.Any())
            {
                return;
            }

            twig.RemoveFromContainer();
            foreach (var r in roastable)
            {
                var roasted = r.GetRoasted();

                if (!hero.AddToBag(roasted))
                    break;

                ((GameObject)r).RemoveFromContainer();
            }
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero)
        {
            var roastingObjects = hero.GetContainerItems()
                .Where(o => o.Properties.Contains(Property.Roastable) && o is IRoastable)
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
