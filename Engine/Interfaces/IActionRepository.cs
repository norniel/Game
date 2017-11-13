using System.Collections.Generic;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;

namespace Engine.Interfaces
{
    interface IActionRepository
    {
        IEnumerable<IAction> GetPossibleActions(Hero hero, GameObject @object);
    }
}
