using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Objects;

namespace Engine.Actions
{
    internal class CollectBerriesAction:CollectSmth<Berry>
    {
        public override string Name => "Collect berries";

        public override bool IsApplicable(Property property)
        {
            return property == Property.CollectBerries;
        }
        /*
        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {   
            // collect
            // change objects
            var actionIsNotOver = objects.Select(o => o.GameObject).OfType<IHasBerries>().Any(hb => this.CollectBerries(hb, hero));

            return !actionIsNotOver;
        }
        */
        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectBerries) && ((IHasSmthToCollect<Berry>)obj).GetSmthTotalCount() > 0);
        }

        public override double GetTiredness()
        {
            return 0.1;
        }

        /*
        private bool CollectBerries(IHasBerries objectWithBerries, Hero hero)
        {
            if (objectWithBerries.BerriesCount <= 0)
                return false;

            int berriesToBagCount = objectWithBerries.BerriesCount < objectWithBerries.BerriesPerCollectCount ? 
                objectWithBerries.BerriesCount :
                objectWithBerries.BerriesPerCollectCount;

            objectWithBerries.BerriesCount = objectWithBerries.BerriesCount < objectWithBerries.BerriesPerCollectCount
                ? 0
                : objectWithBerries.BerriesCount - objectWithBerries.BerriesPerCollectCount;

            var berriestoBag = new List<Berry>();

            for (int i = 0; i < berriesToBagCount; i++)
            {
                berriestoBag.Add(objectWithBerries.GetBerry());
            }
            hero.AddToBag(berriestoBag);

            return objectWithBerries.BerriesCount > 0;
        }
        */
    }
}
