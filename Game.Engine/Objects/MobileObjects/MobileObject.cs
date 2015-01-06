using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Game.Engine
{
    public abstract class MobileObject : FixedObject
    {
        protected Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

        public Point Position { get; set; }

        public IState State { get; protected set; }

        protected Queue<IState> _stateQueue;

        public StateEvent StateEvent { get; private set; }

        public uint Speed { get; set; }

        public double Angle { get; set; }

        public Size ViewSight { get; set; }
        public List<Point> PointList { get; private set; }

        public IObservable<EventPattern<StateEventArgs>> States
        {
            get { return staSubject; }
        }
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

                if (_stateQueue.Count == 0)
                {
                    State = GetNextState();
                }
            });

            Game.Intervals.CombineLatest(States, (tick, state) => state).Subscribe(x =>
            {
                if (State != null && CheckForUnExpected()) 
                    State.Act();
            });
        }

        public override string Name
        {
            get { return "Mobile object"; }
        }

        public virtual IState GetNextState()
        {
            return new Standing();
        }

        public virtual bool CheckForUnExpected()
        {
            return true;
        }
    }
}
