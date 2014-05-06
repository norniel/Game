using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Game.Engine.Interfaces.IActions;
using Game.Engine.Objects;
using Game.Engine.States;
using Game.Engine.Wrapers;

namespace Game.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;
    using Interfaces;

    public class Hero : MobileObject, IPicker
    {
        private Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

        public IState State { get; private set; }

        public uint Speed{ get; set; }

        public double Angle { get; set; }

        private readonly Queue<IState> _stateQueue;

        private Bag Bag;

        private bool _isThen = false;

        public List<WeakReference> PointList{ get; private set;}

        public IObservable<EventPattern<StateEventArgs>> States
        {
            get { return staSubject; }
        }

        public Hero()
        {
            Position = new Point();
            Speed = 2;
            Angle = 0;

            Bag = new Bag();
            
            _stateQueue = new Queue<IState>();
            PointList = new List<WeakReference>();
            State = new Standing(this);
        //    staSubject.OnNext(new Standing(this));


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
                
                if (_stateQueue.Count == 0 /* && state == null*/)
                {
                    State = new Standing(this);
                }
            });
            //staSubject.
        }

       
/*
        private void OnNextState( IState state )
        {
            staSubject.OnNext(state);
            if (_stateQueue.Count == 0 && state == null)
                return;

            if( _stateQueue.Count > 0 )
            {
                IState nextState;
                while ( _stateQueue.Count > 0 )
                {
                    nextState = _stateQueue.Dequeue();

                    if (nextState == State || nextState == null)
                        continue;

                    if (State != null)
                        State.NextState -= OnNextState;

                    State = nextState;
                    State.NextState += OnNextState;

                    return;
                }
                
            }

            if( State != state )
            {
                if (State != null)
                    State.NextState -= OnNextState;

                State = state;
                State.NextState += OnNextState;
            }
        }
*/
        /*
        private void OnNextState(IState state)
        {

        }
        */
        public void StartMove( Point destination, Stack<Point> points )
        {
            if (!_isThen)
            {
                _stateQueue.Clear();
            }
            else
                _isThen = _stateQueue.Count > 0;

            if( points == null )
            {
                _stateQueue.Enqueue(new Moving(this, destination));
                //OnNextState(new Moving(this, destination));
                return;
            }

            PointList.Clear();
            PointList.Add(new WeakReference(Position));
            while( points.Count > 0 )
            {
                PointList.Add( new WeakReference(points.Peek()) );
                _stateQueue.Enqueue(new Moving(this, points.Pop()));
            }

            if (!_isThen)
                StateEvent.FireEvent();

            _isThen = false;
            // OnNextState( null );
        }

        public void AddToBag(IEnumerable<GameObject> objects)
        {
            Bag.Add(objects);
        }

        public List<GameObject> GetContainerItems()
        {
            return Bag.GameObjects;
        }

        public Hero Then()
        {
            this._isThen = true;
            return this;
        }

        public void StartActing(IAction action, Point destination, IEnumerable<RemovableWrapper<GameObject>> objects)
        {
            if (!_isThen)
            {
                _stateQueue.Clear();
            }
            else
                _isThen = _stateQueue.Count > 0;

            _stateQueue.Enqueue(new Acting(this, action, destination, objects));

            if (!_isThen)
                StateEvent.FireEvent();

            _isThen = false;
        }
    }
}