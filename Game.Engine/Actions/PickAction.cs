using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

namespace Game.Engine
{
    internal class PickAction : IAction
    {
        public string Name {
            get { return "Pick"; }
        }
        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            hero.AddToBag(objects);

            return true;
        }
        
        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Pickable));// && hero.HasEmpptyContainer;
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Pickable)).ToList();// && hero.HasEmpptyContainer;
        }

        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Pickable;
        }
    }
}
