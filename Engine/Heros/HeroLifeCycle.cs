using System;
using System.Collections.Generic;
using Engine.Effects;
using Unity.Attributes;

namespace Engine.Heros
{
    public class HeroLifeCycle:IObserver<long>
    {
        private const int INITIAL_HEALTH = 100;
        private const int INITIAL_SATIETY = 100;
        private const int QUOTER_SATIETY = 25;

        private const int INITIAl_TIREDNESS = 0;
        private const int MIDDLE_TIREDNESS = 50;
        private const int STRONG_TIREDNESS = 100;
        private const int FINAL_TIREDNESS = 150;
        private readonly HeroProperties _heroProperties;
        private const uint maxTimeStamp = 2* Game.TimeStep;
        private uint timestamp;

        [Dependency]
        public DayNightCycle DayNightCycle { get; set; }

        public HeroProperties HeroProperties => _heroProperties;

        public int Tiredness => HeroProperties.Tiredness;

        private readonly List<IEffect> _effects = new List<IEffect>();

        public HeroLifeCycle()
        {
            timestamp = maxTimeStamp;

            _heroProperties = new HeroProperties
            {
                Health = INITIAL_HEALTH,
                Satiety = INITIAL_SATIETY,
                InnerTiredNess = INITIAl_TIREDNESS
            };
        }

        public void OnNext(long value)
        {
            if (timestamp < maxTimeStamp)
            {
                timestamp ++;
                return;
            }

            timestamp = 0;

            lock (_heroProperties)
            {
                if (_heroProperties.Satiety > 0)
                    _heroProperties.Satiety--;
                else
                {
                    _heroProperties.Health--;
                }
                
                if (_heroProperties.Health < INITIAL_HEALTH && _heroProperties.Satiety > QUOTER_SATIETY && _heroProperties.Tiredness < STRONG_TIREDNESS)
                {
                    _heroProperties.Health = Math.Min(_heroProperties.Health + 1, INITIAL_HEALTH);
                }

                _heroProperties.Health = Math.Max(_heroProperties.Health, 0);
                
                if (DayNightCycle.IsNight())
                    IncreaseTiredness(5);

                if (DayNightCycle.IsDusk())
                    IncreaseTiredness(1);
            }

            ApplyEffects();
        }

        internal void ApplyToxic(int poisoness)
        {
            lock (_heroProperties)
            {
                _heroProperties.Health = Math.Max(_heroProperties.Health - poisoness, 0);
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void Eat(int satiety)
        {
            if (_heroProperties.Satiety >= INITIAL_SATIETY)
                return;
            
            lock (_heroProperties)
            {
                if (_heroProperties.Satiety < INITIAL_SATIETY)
                {
                    if (_heroProperties.Satiety < INITIAL_SATIETY)
                        _heroProperties.Satiety = Math.Min(_heroProperties.Satiety+ satiety, INITIAL_SATIETY);
                }
            }
        }

        public double GetSpeedCoefficient()
        {
            if (_heroProperties.Tiredness < MIDDLE_TIREDNESS)
                return 1.0;
            
            if (_heroProperties.Tiredness > STRONG_TIREDNESS)
                return 0.25;

            return 1 - (_heroProperties.Tiredness - MIDDLE_TIREDNESS) * 0.75 / MIDDLE_TIREDNESS;
        }

        public void IncreaseTiredness(double value)
        {
            lock (_heroProperties)
            {
                _heroProperties.InnerTiredNess += value;
                _heroProperties.InnerTiredNess = Math.Min(_heroProperties.InnerTiredNess, FINAL_TIREDNESS);
            }
        }

        public void DecreaseTiredness(double value)
        {
            lock (_heroProperties)
            {
                _heroProperties.InnerTiredNess -= value;
                _heroProperties.InnerTiredNess = Math.Max(_heroProperties.InnerTiredNess, INITIAl_TIREDNESS);
            }
        }

        public bool TotallyTired()
        {
            return HeroProperties.Tiredness >= FINAL_TIREDNESS;
        }


        private void ApplyEffects()
        {
            var i = 0;
            while (i < _effects.Count)
            {
                var effect = _effects[i];

                if (effect.Counter <= 0)
                {
                    _effects.RemoveAt(i);
                    continue;
                }

                effect.Counter--;
                effect.Apply(this);
                i++;
            }
        }

        internal void AddEffect(IEffect effect)
        {
            if (effect.Counter <= 0)
                return;

            _effects.Add(effect);
        }

        public void NextGen()
        {
            _heroProperties.Satiety = INITIAL_SATIETY;
            _heroProperties.InnerTiredNess = 0;
            _heroProperties.Health = INITIAL_HEALTH;
            _effects.Clear();
        }
    }
}
