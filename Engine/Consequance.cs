using System;
using Engine.Effects;
using Engine.Heros;
using Engine.Tools;

namespace Engine
{
    static class Consequance
    {
        public static Action<Hero> Probability(double prbab, Action<Hero> action)
        {
            return hero =>
            {
                var random = Game.Random.NextDouble();
                if (prbab > random) action(hero);
            };
        }

        public static Action<Hero> ProbabilityOrElse(double prbab, Action<Hero> action, Action<Hero> elseAction)
        {
            return hero =>
            {
                if (prbab == 1.0 || prbab > Game.Random.NextDouble()) action(hero);
                else elseAction(hero);
            };
        }

        public static Action<Hero> Composite(params Action<Hero>[] actions)
        {
            return hero =>
            {
                foreach (var action in actions)
                {
                    action(hero);
                }
            };
        }

        public static Action<Hero> AddKnowledge(Knowledges knowledge)
        {
            return hero =>
            {
                hero.AddKnowledge(knowledge);
            };
        }

        public static Action<Hero> AddKnowledge(Knowledges knowledge, uint koef)
        {
            return hero =>
            {
                hero.SetKnowledge(knowledge, koef);
            };
        }


        public static Action<Hero> AddObjectKnowledge(string objName, uint knowledgeKoef)
        {
            return hero =>
            {
                hero.SetObjectKnowledge(objName, knowledgeKoef);
            };
        }

        public static Action<Hero> AddToxicEffect(int poisonness, int time)
        {
            return hero =>
            {
                if (poisonness <= 0 || time <= 0)
                    return;

                hero.AddEffect(new ToxicEffect(poisonness, time));
            };
        }
    }

    internal class ConseqActionResult : IActionResult
    {
        public ConseqActionResult(bool b, params Action<Hero>[] conseqs)
        {
            IsFinished = b;
            Conseqs = conseqs;
        }

        public bool IsFinished { get; }
        public void Apply(Hero hero)
        {
            foreach (var conseq in Conseqs)
            {
                conseq(hero);
            }
        }

        public Action<Hero>[] Conseqs { get; }
    }
}
