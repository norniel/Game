using System.Collections.Generic;
using System.Linq;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Objects.LargeObjects;

namespace Game.Engine.Actions
{
    class SleepAction:IAction
    {
        public string Name {
            get { return "Sleep"; }
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
