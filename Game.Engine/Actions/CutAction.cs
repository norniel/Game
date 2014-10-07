using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Game.Engine.Heros;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.Objects.Trees;
using Game.Engine.Wrapers;
using Microsoft.Practices.Unity;

namespace Game.Engine.Actions
{
    public class CutAction : IAction
    {
        [Dependency]
        public Map Map { get; set; }

        public string Name {
            get { return "Cut"; }
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Cuttable || property == Property.Cutter;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            // TODO: implement speed and time of cutting depending on quility and shapeness of cutter and hardness of cuttable
            // TODO: implement damaging of cuttable and damaging of cutter depending of hardness of cuttable

            var cuttableObject = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cuttable));
            var cutter = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cutter));

            if (cuttableObject == null || cutter == null)
            {
                return true;
            }

            if (this.TotalActionTime <= this.elapsedActionTime)
            {
                cuttableObject.RemoveFromContainer();

                Map.SetObjectFromDestination(hero.Position, new Log());
                return true;
            }

            this.elapsedActionTime++;

            return false;
        }

        private int elapsedActionTime = 0;

        private int TotalActionTime {
            get { return 4; } 
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(o => o.Properties.Contains(Property.Cuttable));
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var cuttableObject = objects.FirstOrDefault(o => o.Properties.Contains(Property.Cuttable));

            // TODO: implement choosing cutters with different quility
            var cutter = hero.GetContainerItems().FirstOrDefault(o => o.Properties.Contains(Property.Cutter));

            if (cuttableObject != null && cutter != null)
            {
                yield return new List<GameObject> { cuttableObject, cutter };
            }
        }
    }
}
