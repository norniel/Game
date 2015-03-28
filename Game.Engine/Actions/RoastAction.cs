using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces;
using Game.Engine.Objects;

namespace Game.Engine.Actions
{
    internal class RoastAction : LongActionBase
    {
        public override string Name
        {
            get { return "Roast"; }
        }

        public override string GetName(IEnumerable<GameObject> objects)
        {
            return string.Format("Roast {0}", objects.First().Name);
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
            throw new System.NotImplementedException();
        }

        public override IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
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

        protected override int ElapsedActionTime{ get; set; }

        protected override int TotalActionTime
        {
            get { return 2; }
        }
    }
}
