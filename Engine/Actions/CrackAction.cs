﻿using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Behaviors;
using Engine.Heros;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Objects.Tool;
using Engine.Tools;

namespace Engine.Actions
{
    internal class CrackAction : IAction
    {
        public string Name => "Crack";
        
        public string GetName(IEnumerable<GameObject> objects, Hero hero)
        {
            var gameObject = objects.First(ao => ao.HasBehavior<CrackableBehavior>());
            var name = hero.IsBaseToShow(gameObject) ? gameObject.GetBaseName() : gameObject.Name;

            return $"Crack {name}";
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

        public IEnumerable<IList<GameObject>> GetActionsWithNecessaryObjects(IEnumerable<GameObject> objects, Hero hero, Point actionPosition)
        {
            var allObjects =
                objects.Union(hero.GetContainerItems()).ToList();

            var nut = allObjects.FirstOrDefault(ao => ao.HasBehavior<CrackableBehavior>());
            var stone = allObjects.FirstOrDefault(ao => ao is Rock || ao is SharpStone);
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

        public IActionResult Do(Hero hero, IList<GameObject> objects)
        {
            var gameObjects = objects as GameObject[] ?? objects.ToArray();

            var nut = gameObjects.FirstOrDefault(ao => ao.HasBehavior<CrackableBehavior>());
            var stone = gameObjects.FirstOrDefault(ao => ao is Rock || ao is SharpStone);
            var cracker = gameObjects.Where(ao => ao != stone)
                .FirstOrDefault(ao => ao.Properties.Contains(Property.Cracker));

            if (nut == null || stone == null || cracker == null)
            {
                return FinishedActionResult.Instance;
            }

            var crackableBehavior = nut.GetBehavior<CrackableBehavior>();
            nut.RemoveFromContainer();
            var nutKernel = crackableBehavior?.GetCrackable();

            Game.AddToGame(hero, nutKernel as FixedObject);

            return new ConseqActionResult(true, 
                SharpStoneConseq(0.5, stone), 
                Consequance.AddObjectKnowledge(nut.Name, 5), 
                Consequance.Probability(0.5, Consequance.AddObjectKnowledge(nutKernel?.Name, 1)));
        }

        private static Action<Hero> SharpStoneConseq(double prbab, GameObject stone)
        {
            return Consequance.Probability(prbab,
                Consequance.Composite(
                    CreateSharpStone.Create(stone),
                    Consequance.AddKnowledge(Knowledges.CreateSharpStone)));
        }

   /*     
        class ProbabCompositeConseq : ProbabConseqBase
        {
            
        }

        private abstract class ProbabConseqBase : IConseq
        {
            private readonly double _probab;

            public ProbabConseqBase(double probab)
            {
                _probab = probab;
            }
            public void Apply(Hero hero)
            {
                if (Game.Random.NextDouble() < _probab)
                {
                    ApplyImpl(hero);
                }
            }

            protected abstract void ApplyImpl(Hero hero);
        }

        class CreateObjetConseq : ProbabConseqBase
        {
            public GameObject Object { get; set; }
        }

        class KnowledgeConseq : IConseq
        {
            public Knowledges Knowledge { get; set; }
            public void Apply(Game game)
            {
                game.AddKnowledge(Knowledge);
            }
        }
        */
    }
    /*
    internal interface IConseq
    {
        void Apply(Hero hero);
    }*/
}

