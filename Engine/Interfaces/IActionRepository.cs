using System.Collections.Generic;
using Engine.Interfaces.IActions;
using Engine.Objects;

namespace Engine.Interfaces
{
    interface IActionRepository
    {
        IEnumerable<IAction> GetPossibleActions(GameObject @object);
    }
}
