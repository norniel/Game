using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Actions
{
    public class CreateStoneAxeAction:IAction
    {
        public string Name {
            get { return "Create Stone axe"; }
            private set { }
        }
        public bool IsApplicable(Property property)
        {
            return 
                Property.NeedToCreateStoneAxe == property;
        }

        public bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            throw new System.NotImplementedException();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.OfType<Branch>().Any();
        }

        public IEnumerable<List<RemovableWrapper<GameObject>>> GetActionsWithNecessaryObjects(IEnumerable<RemovableWrapper<GameObject>> objects, Hero hero)
        {
            var allObjects = objects.Union(hero.GetContainerItemsAsRemovable());
            var branch = allObjects.FirstOrDefault(ao => ao.GameObject is Branch);
            var stone = allObjects.FirstOrDefault(ao => ao.GameObject is Rock);

            if (branch != null && stone != null)
            {
                yield return new List<RemovableWrapper<GameObject>> { branch, stone };
            }
        }
    }
}
