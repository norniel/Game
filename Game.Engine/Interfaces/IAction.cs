using System.Collections.Generic;
using Game.Engine.Objects;
using Game.Engine.Wrapers;

namespace Game.Engine.Interfaces.IActions
{
    public interface IAction
    {
        string Name { get; }

        bool IsApplicable(Property property);

        bool Do(Hero hero, IEnumerable<RemovableWrapper<GameObject>>objects);

        bool CanDo(Hero hero, IEnumerable<GameObject> objects);
    }
}
