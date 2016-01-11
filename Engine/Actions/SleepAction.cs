using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.LargeObjects;
using Engine.Resources;

namespace Engine.Actions
{
    class SleepAction:IAction
    {
        public string Name {
            get { return ActionsResource.Sleep; }
        }
        public string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.NeedToSleep;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            hero.Sleep();
            return true;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var grassBed = (GrassBed)objects.FirstOrDefault(o => o is GrassBed);

            if (grassBed != null && grassBed.IsBuild)
            {
                yield return new List<GameObject>() {grassBed};
            }
        }

        public double GetTiredness()
        {
            return 0;
        }
    }
}
