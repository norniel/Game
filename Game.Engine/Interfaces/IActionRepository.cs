using System.Collections.Generic;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;

namespace Game.Engine.Interfaces
{
    interface IActionRepository
    {
        IEnumerable<IAction> GetPossibleActions(GameObject @object);
    }
}
