using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Food;
using Engine.Objects.Fruits;

namespace Engine.Actions
{
    internal class CrackAction : IAction
    {
        public string Name => "Crack";

        public string GetName(IEnumerable<GameObject> objects)
        {
            return string.Format("Crack", objects.OfType<Nut>().First().Name);
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Crackable ||
                property == Property.Cracker ||
                property == Property.Stone;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<List<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).ToList();

            var nut = allObjects.FirstOrDefault(ao => ao is Nut);
            var stone = allObjects.FirstOrDefault(ao => ao is Rock);
            var cracker = allObjects.Where(ao => ao != stone)
                .FirstOrDefault(ao => ao.Properties.Contains(Property.Cracker));

            if (nut != null && stone != null && cracker != null)
            {
                yield return new List<GameObject> { nut, stone, cracker };
            }
        }

        public double GetTiredness()
        {
            return 1;
        }

        public Point GetDestination(Point destination, FixedObject destObject, Hero hero)
        {
            return destination;
        }

        public bool Do(Hero hero, IEnumerable<GameObject> objects)
        {
            var nut = objects.FirstOrDefault(ao => ao is Nut);
            var stone = objects.FirstOrDefault(ao => ao is Rock);
            var cracker = objects.Where(ao => ao != stone)
                .FirstOrDefault(ao => ao.Properties.Contains(Property.Cracker));

            if (nut == null || stone == null || cracker == null)
            {
                return true;
            }

            nut.RemoveFromContainer();
            var nutKernel = new NutKernel();

            if (!hero.AddToBag(nutKernel))
            {
                Game.Map.SetHObjectFromDestination(hero.Position, nutKernel);
            }

            return true;
        }
    }
}
