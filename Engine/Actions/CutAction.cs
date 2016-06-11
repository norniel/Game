using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Trees;
using Engine.Resources;
using Engine.Wrapers;
using Microsoft.Practices.Unity;

namespace Engine.Actions
{
    class CutAction : LongActionBase
    {
        // TODO: implement speed and time of cutting depending on quility and shapeness of cutter and hardness of cuttable
        // TODO: implement damaging of cuttable and damaging of cutter depending of hardness of cuttable

        [Dependency]
        public Map Map { get; set; }

        public override string Name {
            get { return ActionsResource.Cut; }
        }

        public override string GetName(IEnumerable<GameObject> objects)
        {
            return Name;
        }

        public override bool IsApplicable(Property property)
        {
            return property == Property.Cuttable || property == Property.Cutter;
        }

        protected override bool DoNotLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var cuttableObject = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cuttable));
            var cutter = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cutter));

            if (cuttableObject == null || cutter == null)
            {
                return true;
            }

            return false;
        }

        protected override void DoLast(Hero hero, IEnumerable<GameObject> objects)
        {
            var cuttableObject = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cuttable));
            var cutter = objects.SingleOrDefault(o => o.Properties.Contains(Property.Cutter));

            if (cuttableObject == null || cutter == null)
            {
                return;
            }

            cuttableObject.RemoveFromContainer();

            Map.SetHObjectFromDestination(hero.Position, new Log());
            return;

        }

        protected override int TotalActionTime {
            get { return 4; } 
        }

        public override bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            return objects.All(o => o.Properties.Contains(Property.Cuttable));
        }

        public override IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var cuttableObject = objects.FirstOrDefault(o => o.Properties.Contains(Property.Cuttable));

            // TODO: implement choosing cutters with different quility
            var cutter = hero.GetContainerItems().FirstOrDefault(o => o.Properties.Contains(Property.Cutter));

            if (cuttableObject != null && cutter != null)
            {
                yield return new List<GameObject> { cuttableObject, cutter };
            }
        }

        public override double GetTiredness()
        {
            return 3;
        }

        protected override int ElapsedActionTime{ get; set; }
    }
}
