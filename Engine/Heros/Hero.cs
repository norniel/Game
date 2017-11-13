using Engine.Resources;

namespace Engine.Heros
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Interfaces.IActions;
    using Objects;
    using States;
    public class Hero : MobileObject, IPicker, IEater
    {
      //  private Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

      //  public uint Speed { get; set; }

     //   public double Angle { get; set; }

        private const int INITIAL_SPEED = 20;

        private readonly Bag _bag;

        private readonly HeroLifeCycle _heroLifeCycle;

        internal HeroLifeCycle HeroLifeCycle => _heroLifeCycle;

        private bool _isThen;

        private HashSet<Knowledges> _knowledgeses = new HashSet<Knowledges> {Knowledges.Nothing};

        public Hero()
        {
          //  Position = new Point();
            Angle = 0;

            _bag = new Bag(20, 20);

            _heroLifeCycle = new HeroLifeCycle();

            //todo - extract to method

            Game.Intervals.Subscribe(HeroLifeCycle);
        }

        public IMap Map { get; set; }

        public Bag Bag => _bag;

        public void StartMove(Point destination, Stack<Point> points)
        {
            using (new StateFirer(this))
            {
                if (points == null)
                {
                    _stateQueue.Enqueue(new Moving(this, destination));
                    return;
                }

                PointList.Clear();
                PointList.Add(Position);
                while (points.Count > 0)
                {
                    PointList.Add(points.Peek());
                    _stateQueue.Enqueue(new Moving(this, points.Pop()));
                }
            }
        }

        public bool AddToBag(GameObject gameObject)
        {
            return _bag.Add(gameObject);
        }
        public void AddToBag(IEnumerable<GameObject> objects)
        {
            _bag.Add(objects);
        }

        public void GetFromBag()
        {

        }

        public List<GameObject> GetContainerItems()
        {
            return _bag.GameObjects;
        }
        /*
        public List<GameObject> GetContainerItemsAsRemovable()
        {
            return Bag.GameObjects.Select(go => new RemovableWrapper<GameObject>
            {
                GameObject = go,
                RemoveFromContainer = (() =>
                {
                    this.RemoveFromContainer(go);
                })
            }).ToList();
        }
        */
        public Hero Then()
        {
            this._isThen = true;
            return this;
        }

        public void StartActing(IAction action, Point destination, IEnumerable<GameObject> objects)
        {
            using (new StateFirer(this))
            {
                _stateQueue.Enqueue(new Acting(this, action, destination, objects));
            }
        }

        class StateFirer : IDisposable
        {
            private readonly Hero _hero;
            public StateFirer(Hero hero)
            {
                _hero = hero;
                if (!_hero._isThen)
                {
                    _hero._stateQueue.Clear();
                }
                else
                    _hero._isThen = _hero._stateQueue.Count > 0;
            }
            public void Dispose()
            {
                if (!_hero._isThen)
                    _hero.StateEvent.FireEvent();

                _hero._isThen = false;
            }
        }

        public void Eat(int satiety)
        {
            this.HeroLifeCycle.Eat(satiety);
        }

        public IEnumerable<KeyValuePair<string, int>> GetProperties()
        {
            return new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>(HeroResource.Health, _heroLifeCycle.HeroProperties.Health),
                new KeyValuePair<string, int>(HeroResource.Satiety, _heroLifeCycle.HeroProperties.Satiety),
                new KeyValuePair<string, int>(HeroResource.Tiredness, _heroLifeCycle.HeroProperties.Tiredness)
            };
        }

        public override uint Speed
        {
            get { return (uint)(INITIAL_SPEED *_heroLifeCycle.GetSpeedCoefficient()); }
            set {  }
        }

        public void Sleep()
        {
            _stateQueue.Clear();
            _stateQueue.Enqueue(new Sleeping(this));
        }

        public void FallUnconscios()
        {
            _stateQueue.Clear();
            _stateQueue.Enqueue(new Unconscios(this));
            StateEvent.FireEvent();
        }

        public override bool CheckForUnExpected()
        {
            if (!HeroLifeCycle.TotallyTired() || State is Unconscios)
                return true;

            FallUnconscios();

            return false;
        }

        public bool IsUnconscios()
        {
            return State is Unconscios;
        }

        public bool IsHorizontal()
        {
            return State is Unconscios || State is Sleeping;
        }

        public void AddKnowledge(Knowledges knowledge)
        {
            _knowledgeses.Add(knowledge);
        }

        public bool HasKnowledge(Knowledges knowledge)
        {
            return _knowledgeses.Contains(knowledge);
        }
    }
}