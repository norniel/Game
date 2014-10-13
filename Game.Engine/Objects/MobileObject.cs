﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Game.Engine.Objects;

namespace Game.Engine
{
    public abstract class MobileObject : FixedObject
    {
        protected Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

        public Point Position { get; set; }

        public IState State { get; protected set; }

        protected Queue<IState> _stateQueue;

        public uint Speed { get; set; }

        public double Angle { get; set; }
            
        public List<Point> PointList { get; private set; }

        public IObservable<EventPattern<StateEventArgs>> States
        {
            get { return staSubject; }
        }
        public MobileObject()
        {
            Position = new Point();
            Speed = 2;
            Angle = 0;

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

                        if (nextState == State || nextState == null)
                            continue;

                        State = nextState;

                        return;
                    }
                }

                if (_stateQueue.Count == 0)
                {
                    State = new Standing();
                }
            });

            Game.Intervals.CombineLatest(States, (tick, state) => state).Subscribe(x => { if (State != null) State.Act(); });
        }

        public override string Name
        {
            get { return "Mobile object"; }
        }
    }
}
