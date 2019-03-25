﻿using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Effects;
using Engine.Interfaces;
using Engine.Interfaces.IActions;
using Engine.Objects;
using Engine.Resources;
using Engine.States;
using Unity;

namespace Engine.Heros
{
    public class Hero : MobileObject, IEater
    {
        private const int INITIAL_SPEED = 50;

        [Dependency] public HeroLifeCycle HeroLifeCycle { get; set; }

        private bool _isThen;

        private Dictionary<Knowledges, uint> _knowledges =
            new Dictionary<Knowledges, uint> {{Knowledges.Nothing, 100}};

        private Dictionary<string, uint> _objectKnowledges = new Dictionary<string, uint>();

        public Hero()
        {
            //  Position = new Point();
            Angle = 0;

            Bag = new Bag(20, 20);

            // _heroLifeCycle = new HeroLifeCycle();

            //todo - extract to method

            // Game.Intervals.Subscribe(HeroLifeCycle);
        }

        public Bag Bag { get; }

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
            return Bag.Add(gameObject);
        }

        public void AddToBag(IEnumerable<GameObject> objects)
        {
            Bag.Add(objects);
        }

        public List<GameObject> GetContainerItems()
        {
            return Bag.GameObjects;
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
            _isThen = true;
            return this;
        }

        public void StartActing(IAction action, Point destination, IList<GameObject> objects)
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
            HeroLifeCycle.Eat(satiety);
        }

        public EaterType EaterType => EaterType.Human;

        public IEnumerable<KeyValuePair<string, int>> GetProperties()
        {
            return new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>(HeroResource.Health, HeroLifeCycle.HeroProperties.Health),
                new KeyValuePair<string, int>(HeroResource.Satiety, HeroLifeCycle.HeroProperties.Satiety),
                new KeyValuePair<string, int>(HeroResource.Tiredness, HeroLifeCycle.HeroProperties.Tiredness)
            };
        }

        public override uint Speed
        {
            get => (uint) (INITIAL_SPEED * HeroLifeCycle.GetSpeedCoefficient());
            set { }
        }

        public void Sleep()
        {
            _stateQueue.Clear();
            _stateQueue.Enqueue(new Sleeping(this));
        }

        private void FallUnconscios()
        {
            _stateQueue.Clear();
            _stateQueue.Enqueue(new Unconscios(this));
            StateEvent.FireEvent();
        }

        private void Halt()
        {
            _stateQueue.Clear();
            _stateQueue.Enqueue(new Halting(this));
            _stateQueue.Enqueue(new Halt());
            StateEvent.FireEvent();
        }

        public override bool CheckForUnExpected()
        {
            if (HeroLifeCycle.HeroProperties.Health <= 0 && !(State is Halting) && !(State is Halt))
            {
                Halt();
                return false;
            }

            if (HeroLifeCycle.TotallyTired() && !(State is Unconscios))
            {
                FallUnconscios();
                return false;
            }

            return true;
        }

        public bool IsUnconscios()
        {
            return State is Unconscios || State is Halting || State is Halt;
        }

        public bool IsHalt()
        {
            return State is Halt;
        }

        public bool IsHorizontal()
        {
            return IsUnconscios() || State is Sleeping;
        }

        public void AddKnowledge(Knowledges knowledge)
        {
            SetKnowledge(knowledge, 1);
        }

        public bool HasKnowledge(Knowledges knowledge)
        {
            return _knowledges.ContainsKey(knowledge);
        }

        public double GetObjectKnowledge(string gameObjectName)
        {
            uint objectKnowl;
            return _objectKnowledges.TryGetValue(gameObjectName, out objectKnowl) ? objectKnowl / 100.0 : 0.0;
        }

        public void SetObjectKnowledge(string gameObjectName, uint knowledge)
        {
            if (string.IsNullOrEmpty(gameObjectName))
                return;

            if (_objectKnowledges.ContainsKey(gameObjectName))
            {
                var prevKnowledge = _objectKnowledges[gameObjectName];
                _objectKnowledges[gameObjectName] = Math.Min(100, _objectKnowledges[gameObjectName] + knowledge);
            }
            else
            {
                _objectKnowledges[gameObjectName] = Math.Min(100, knowledge);
            }
        }

        public void SetKnowledge(Knowledges knowledge, uint koef)
        {
            if (_knowledges.ContainsKey(knowledge))
            {
                var prevKnowledge = _knowledges[knowledge];
                _knowledges[knowledge] = Math.Min(100, _knowledges[knowledge] + koef);
            }
            else
            {
                _knowledges[knowledge] = Math.Min(100, koef);
            }
        }

        public double GetKnowledge(Knowledges knowledge)
        {
            uint koef;
            return _knowledges.TryGetValue(knowledge, out koef) ? koef / 100.0 : 0.0;
        }

        public Dictionary<string, uint> GetAllKnowledges()
        {
            return _knowledges.Select(k => new KeyValuePair<string, uint>(k.Key.ToString(), k.Value))
                .Where(t => !string.Equals(t.Key, Knowledges.Nothing.ToString()))
                .Union(_objectKnowledges.Select(t => t)).ToDictionary(t => t.Key, t => t.Value);
        }

        public bool IsBaseToShow(GameObject gameObject)
        {
            return Game.IsShowBase && gameObject.NeedKnowledge &&
                   GetObjectKnowledge(gameObject.Name) < gameObject.KnowledgeKoef;
        }

        public override void EnqueueNextState()
        {
            _stateQueue.Enqueue(new StandingHero(this));
        }

        internal void AddEffect(IEffect effect)
        {
            HeroLifeCycle.AddEffect(effect);
        }

        internal void RewriteKnowledges(Dictionary<string, uint> newKnowledges)
        {
            var allKnowledges = newKnowledges.Where(t => t.Value > 0).Select(t =>
            {
                Knowledges knowledge;
                var knowledgeExist = Enum.TryParse(t.Key, true, out knowledge);
                return new
                {
                    t.Key,
                    KnKey = knowledge,
                    KnowledgeExist = knowledgeExist,
                    t.Value
                };
            }).ToList();

            _knowledges = allKnowledges.Where(t => t.KnowledgeExist).ToDictionary(t => t.KnKey, t => t.Value);
            _knowledges[Knowledges.Nothing] = 100;

            _objectKnowledges = _objectKnowledges.Join(allKnowledges.Where(t => !t.KnowledgeExist), t => t.Key, u => u.Key,
                    (pair, kn) => new KeyValuePair<string, uint>(pair.Key, kn.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            HeroLifeCycle.NextGen();

            _stateQueue.Clear();
            StateEvent.FireEvent();
        }

        public override bool IsMoving
        {
            get { return State is Moving; }
        }
    }
}