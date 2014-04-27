using System.Collections.Generic;
using Game.Engine.Objects;

namespace Game.Engine.Interfaces.IActions
{
    interface IAction
    {
        string Name { get; }

        bool IsApplicable(Property property);

        void Do(Hero hero, IEnumerable<GameObject> objects);

        bool CanDo(Hero hero, IEnumerable<GameObject> objects);
    }
}
