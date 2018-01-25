using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Engine
{
    public abstract class MobileObject : FixedObject
    {
        protected Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

        private Point _position;
        private Point _positionCell;
        private bool _positionChanged = true;

        public Point Position {
            get => _position;
            set
            {
                _position = value;
                _positionChanged = true;
            }
        }

        public Point PositionCell
        {
            get
            {
                if (_positionChanged)
                {
                    _positionCell = Map.PointToCell(_position);
                    _positionChanged = false;
                }

                return _positionCell;
            }
        }

        public IState State { get; protected set; }

        protected Queue<IState> _stateQueue;

        public StateEvent StateEvent { get; }

        public virtual uint Speed { get;set; }

        public double Angle { get; set; }

        public int ViewRadius { get; set; }

        public Size ViewSight { get; set; }
        public List<Point> PointList { get; }

        public IObservable<EventPattern<StateEventArgs>> States => staSubject;

        public MobileObject()
        {
            ViewSight = new Size();
            Position = new Point();
            Speed = 2;
            Angle = 0;

            StateEvent = new StateEvent();
            _stateQueue = new Queue<IState>();
            PointList = new List<Point>();
            State = new Standing();

            Observable.FromEventPattern<StateHandler, StateEventArgs>(
                ev => StateEvent.NextState += ev,
                ev => StateEvent.NextState -= ev).Subscribe(staSubject);
            staSubject.Subscribe(x =>
            {   
                if (_stateQueue.Count == 0)
                {
                    EnqueueNextState();
                }

                if (_stateQueue.Count > 0)
                {
                    IState nextState;
                    while (_stateQueue.Count > 0)
                    {
                        nextState = _stateQueue.Dequeue();

                        if (nextState != State && nextState != null)
                        {
                            State = nextState;

                            return;
                        }
                    }
                }


            });

            Game.Intervals.CombineLatest(States, (tick, state) => state).Subscribe(x =>
            {
                if (State != null && CheckForUnExpected()) 
                    State.Act();
            });
        }

        public override string Name => "Mobile object";

        public virtual void EnqueueNextState()
        {
            _stateQueue.Enqueue(new Standing());
        }

        public virtual bool CheckForUnExpected()
        {
            return true;
        }
    }
}
