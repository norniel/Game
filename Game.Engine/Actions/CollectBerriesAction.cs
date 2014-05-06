using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    internal class CollectBerriesAction:IAction
    {
        public string Name {
            get { return "Collect berries"; }
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.CollectBerries;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {   
            // collect
            // change objects
            var actionIsNotOver = objects.Select(o => o.GameObject).OfType<IHasBerries>().Any(hb => this.CollectBerries(hb, hero));

            return !actionIsNotOver;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectBerries) && ((IHasBerries)obj).BerriesCount > 0);
        }

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
    }
}
