using Engine.Tools;

namespace Engine.Interfaces.IActions
{
    using System.Collections.Generic;
    using Heros;
    using Objects;
    
    public interface IAction
    {
        //TODO Add method FillContext and CheckContext
        string Name { get; }

        string GetName(IEnumerable<GameObject>objects);

        bool IsApplicable(Property property);

        IActionResult Do(Hero hero, IEnumerable<GameObject>objects);

        bool CanDo(Hero hero, IEnumerable<GameObject> objects);
        IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero);
        double GetTiredness();
        Point GetDestination(Point destination, FixedObject destObject, Hero hero);
        Knowledges GetKnowledge();
    }
}
