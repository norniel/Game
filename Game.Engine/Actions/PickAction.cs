using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine
{
    internal class PickAction : IAction
    {
        public string Name {
            get { return "Pick"; }
        }
        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            hero.AddToBag(objects.Select(o => o.GameObject));

            foreach (var removableObject in objects)
            {
                removableObject.RemoveFromContainer();
            }

            return true;
        }
        
        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Pickable));// && hero.HasEmpptyContainer;
        }

        public IEnumerable<List<RemovableWrapper<GameObject>>> GetActionsWithNecessaryObjects(IEnumerable<RemovableWrapper<GameObject>> objects, Hero hero)
        {
            yield return objects.Where(x => x.GameObject.Properties.Contains(Property.Pickable)).ToList();// && hero.HasEmpptyContainer;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Pickable;
        }
    }
}
