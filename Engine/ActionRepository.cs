using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Unity.Attributes;

namespace Engine
{
    class ActionRepository : IActionRepository
    {
        [Dependency]
        public IAction[] Actions { get; set; }

        public IEnumerable<IAction> GetPossibleActions(Hero hero, GameObject gameObject)
        {
            var properties = gameObject.Properties;

            var result = from action in Actions
                         from property in properties
                         where action.IsApplicable(property)
                         && hero.HasKnowledge(action.GetKnowledge())
                         select action;

            return result.Distinct();

        }
    }
}
