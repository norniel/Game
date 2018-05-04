using System;

namespace Engine.Heros
{
    internal class HeroLifeCycle:IObserver<long>
    {
        private const int INITIAL_HEALTH = 100;
        private const int INITIAL_SATIETY = 100;
        private const int INITIAl_TIREDNESS = 0;
        private const int MIDDLE_TIREDNESS = 50;
        private const int STRONG_TIREDNESS = 100;
        private const int FINAL_TIREDNESS = 150;
        private readonly HeroProperties _heroProperties;
        private const uint maxTimeStamp = 20000 / Game.TimeStep;
        private uint timestamp;

        public HeroProperties HeroProperties => _heroProperties;

        public int Tiredness => HeroProperties.Tiredness;

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

                if (_heroProperties.PoisonesTime > 0)
                {
                    _heroProperties.PoisonesTime--;
                    _heroProperties.Health = Math.Max(_heroProperties.Health - _heroProperties.Poisoness, 0);

                    if (_heroProperties.PoisonesTime == 0)
                    {
                        _heroProperties.PoisonesTime = 0;
                        _heroProperties.Poisoness = 0;
                    }
                }
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

        public void Eat(int satiety, int poisoness, int time)
        {
            if (_heroProperties.Satiety >= INITIAL_SATIETY && _heroProperties.Health >= INITIAL_HEALTH && poisoness == 0)
                return;
            
            lock (_heroProperties)
            {
                _heroProperties.Poisoness = poisoness;
                _heroProperties.PoisonesTime = time;

                if (_heroProperties.Satiety < INITIAL_SATIETY || _heroProperties.Health < INITIAL_HEALTH)
                {
                    if (_heroProperties.Satiety < INITIAL_SATIETY)
                        _heroProperties.Satiety = Math.Min(_heroProperties.Satiety+ satiety, INITIAL_SATIETY);
                    if (_heroProperties.Health < INITIAL_HEALTH)
                        _heroProperties.Health = Math.Min(_heroProperties.Health + satiety, INITIAL_HEALTH);
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
            _heroProperties.InnerTiredNess += value;
            _heroProperties.InnerTiredNess = Math.Min(_heroProperties.InnerTiredNess, FINAL_TIREDNESS);
        }

        public void DecreaseTiredness(double value)
        {
            _heroProperties.InnerTiredNess -= value;
            _heroProperties.InnerTiredNess = Math.Max(_heroProperties.InnerTiredNess, INITIAl_TIREDNESS);
        }

        public bool TotallyTired()
        {
            return HeroProperties.Tiredness >= FINAL_TIREDNESS;
        }
    }
}
