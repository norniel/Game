using System.Reactive.Subjects;
using Game.Engine.Objects;

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
        private Subject<IState> staSubject = new Subject<IState>();

        public IState State { get; private set; }

        public uint Speed{ get; set; }

        public double Angle { get; set; }

        private readonly Queue<IState> _stateQueue;

        private Bag Bag;

        public List<WeakReference> PointList{ get; private set;}

        public Hero()
        {
            Position = new Point();
            Speed = 2;
            Angle = 0;

            Bag = new Bag();
            
            _stateQueue = new Queue<IState>();
            PointList = new List<WeakReference>();

            staSubject.OnNext(new Standing(this));
        }

       

        private void OnNextState( IState state )
        {

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

        
        public void StartMove( Point destination, Stack<Point> points )
        {
            _stateQueue.Clear();
            if( points == null )
            {
                OnNextState(new Moving(this, destination));
                return;
            }

            PointList.Clear();
            PointList.Add(new WeakReference(Position));
            while( points.Count > 0 )
            {
                PointList.Add( new WeakReference(points.Peek()) );
                _stateQueue.Enqueue(new Moving(this, points.Pop()));
            }

            OnNextState( null );
        }

        public void AddToBag(IEnumerable<GameObject> objects)
        {
            Bag.Add(objects);
        }

        public List<GameObject> GetContainerItems()
        {
            return Bag.GameObjects;
        }
    }
}