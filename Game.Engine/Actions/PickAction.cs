using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

namespace Game.Engine
{
    internal class PickAction : IAction
    {
        public string Name {
            get { return "Pick"; }
        }
        public void Do(Hero hero, IEnumerable<GameObject> objects)
        {
            hero.AddToBag(objects);
            // remove objects form location
        }
        
        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Pickable));// && hero.HasEmpptyContainer;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Pickable;
        }
    }
}
