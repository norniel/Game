using System.Collections.Generic;
using System.Linq;
using Game.Engine.Interfaces;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Microsoft.Practices.Unity;

namespace Game.Engine
{
    class ActionRepository : IActionRepository
    {
        [Dependency]
        public IAction[] Actions { get; set; }

        public IEnumerable<IAction> GetPossibleActions(GameObject gameObject)
        {
            var properties = gameObject.Properties;

            var result = from action in Actions
                         from property in properties
                         where action.IsApplicable(property)
                         select action;

            return result.Distinct();

        }
    }
}
