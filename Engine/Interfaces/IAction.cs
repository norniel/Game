using System.Collections.Generic;
using Engine.Heros;
using Engine.Objects;

namespace Engine.Interfaces.IActions
{
    public interface IAction
    {
        //TODO Add method FillContext and CheckContext
        string Name { get; }

        string GetName(IEnumerable<GameObject>objects);

        bool IsApplicable(Property property);

        bool Do(Hero hero, IList<GameObject> objects);

        bool CanDo(Hero hero, IEnumerable<GameObject> objects);
        IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects,
            Hero hero);
        double GetTiredness();
        Point GetDestination(Point destination, FixedObject destObject, Hero hero);
        Knowledges GetKnowledge();
    }
}
