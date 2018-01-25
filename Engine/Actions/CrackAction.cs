using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Food;
using Engine.Objects.Fruits;
using Engine.Objects.Tool;

namespace Engine.Actions
{
    internal class CrackAction : IAction
    {
        public string Name => "Crack";

        private readonly Random _random = new Random();

        public string GetName(IEnumerable<GameObject> objects)
        {
            return $"Crack {objects.OfType<Nut>().First().Name}";
        }

        public bool IsApplicable(Property property)
        {
            return property == Property.Crackable ||
                property == Property.Cracker ||
                property == Property.Stone;
        }

        public bool CanDo(Hero hero, IEnumerable<GameObject> objects)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero)
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

        public Knowledges GetKnowledge()
        {
            return Knowledges.Nothing;
        }

        public bool Do(Hero hero, IList<GameObject> objects)
        {
            var gameObjects = objects as GameObject[] ?? objects.ToArray();

            var nut = gameObjects.FirstOrDefault(ao => ao is Nut);
            var stone = gameObjects.FirstOrDefault(ao => ao is Rock);
            var cracker = gameObjects.Where(ao => ao != stone)
                .FirstOrDefault(ao => ao.Properties.Contains(Property.Cracker));

            if (nut == null || stone == null || cracker == null)
            {
                return true;
            }

            nut.RemoveFromContainer();
            var nutKernel = new NutKernel();

            Game.AddToGame(hero, nutKernel);

            if (_random.Next(3) == 0)
            {
                stone.RemoveFromContainer();
                var sharpStone = new SharpStone();
                Game.AddToGame(hero, sharpStone);
                hero.AddKnowledge(Knowledges.CreateSharpStone);
            }

            return true;
        }
    }
}
