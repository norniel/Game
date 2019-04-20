using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.Tools;
using Unity;

namespace Engine.Actions
{
    internal class DropAction : IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name => ActionsResource.Drop;

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            return Name;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Dropable;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            foreach (var removableObject in objects)
            {
                Map.SetHObjectFromDestination(hero.Position, removableObject as FixedObject);
              //  removableObject.RemoveFromContainer();
            }

            return FinishedActionResult.Instance;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(x => x.Properties.Contains(Property.Dropable));
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero, Point actionPosition)
        {
            yield return objects.Where(x => x.Properties.Contains(Property.Dropable)).ToList();
        }

        public double GetTiredness()
        {
            return 0.1;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
