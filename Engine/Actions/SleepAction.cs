using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Resources;
using Engine.Tools;

namespace Engine.Actions
{
    class SleepAction:IAction
    {
        public string Name => ActionsResource.Sleep;

        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToSleep;
        }

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            hero.Sleep();
            return new FinishedActionResult();
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var bed = (LargeObjectInner)objects.FirstOrDefault(o => o.Properties.Any(IsApplicable));

            if (bed != null && bed.IsBuild)
            {
                yield return new List<GameObject> { bed };
            }
        }

        public double GetTiredness()
        {
            return 0;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }
    }
}
