using System;
using System.Reactive;

namespace Game.Engine.Heros
{
    internal class HeroLifeCycle:IObserver<long>
    {
        private const int INITIAL_HEALTH = 100;
        private const int INITIAL_SATIETY = 100;
        private readonly HeroProperties _heroProperties;
        private const uint maxTimeStamp = 20000 / Game.TimeStep;
        private uint timestamp;

        public HeroProperties HeroProperties {
            get { return _heroProperties; }
        }

        public HeroLifeCycle()
        {
            this.timestamp = maxTimeStamp;

            _heroProperties = new HeroProperties
            {
                Health = INITIAL_HEALTH,
                Satiety = INITIAL_SATIETY
            };
        }

        public void OnNext(long value)
        {
            if (this.timestamp < maxTimeStamp)
            {
                this.timestamp ++;
                return;
            }

            this.timestamp = 0;

            lock (_heroProperties)
            {
                if (_heroProperties.Satiety > 0)
                    _heroProperties.Satiety--;
                else
                {
                    _heroProperties.Health--;
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

        public void Eat(int satiety)
        {
            if (_heroProperties.Satiety >= INITIAL_SATIETY && _heroProperties.Health >= INITIAL_HEALTH)
                return;

            lock (_heroProperties)
            {
                if (_heroProperties.Satiety < INITIAL_SATIETY || _heroProperties.Health < INITIAL_HEALTH)
                {
                    if (_heroProperties.Satiety < INITIAL_SATIETY)
                        _heroProperties.Satiety = Math.Min(_heroProperties.Satiety+ satiety, INITIAL_SATIETY);
                    if (_heroProperties.Health < INITIAL_HEALTH)
                        _heroProperties.Health = Math.Min(_heroProperties.Health + satiety, INITIAL_HEALTH);
                }
            }
        }
    }
}
