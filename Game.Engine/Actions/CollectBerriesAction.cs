using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

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

        public void Do(Hero hero, IEnumerable<GameObject> objects)
        {   
            // collect
            // change objects
            var actionIsNotOver = objects.OfType<IHasBerries>().All(hb => this.CollectBerries(hb, hero));

            if (actionIsNotOver)
            {
                //hero.
            }
            {
                
            }

            // hero.AddToBag();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.Any(obj => obj.Properties.Contains(Property.CollectBerries));
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
