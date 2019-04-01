using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Engine.Tools;

namespace Engine
{
    public abstract class MobileObject : FixedObject
    {
        protected Subject<EventPattern<StateEventArgs>> staSubject = new Subject<EventPattern<StateEventArgs>>();

        private Point _position;
        private Point _positionCell;
        private bool _positionChanged = true;
        private int _drawWidth = 32;

        protected List<IDisposable> _disposables = new List<IDisposable>();

        public Point Position {
            get => _position;
            set
            {
                _position = value;
                _positionChanged = true;

                if(Game.Map == null)
                    return;

                var drawPosition= Map.PointToCell(new Point(Math.Min(_position.X + _drawWidth / 2, Map.MAP_WIDTH - 1), _position.Y));
                drawPosition.Y = Math.Max(0, drawPosition.Y-1);

                if (drawPosition == _drawPosition)
                    return;

                var oldPosition = _drawPosition;

                _drawPosition = drawPosition;
                Game.Map.AddMobileObjectToCell(_drawPosition, this);

                if (oldPosition != null)
                    Game.Map.RemoveMobileObjectFromCell(oldPosition, this);
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

        private Point _drawPosition;

        public IState State { get; protected set; }

        protected Queue<IState> _stateQueue;

        public StateEvent StateEvent { get; }

        public virtual uint Speed { get;set; }

        public double Angle { get; set; }

        public int ViewRadius { get; set; }

        public Size ViewSight { get; set; }
        public List<Point> PointList { get; }

        public IObservable<EventPattern<StateEventArgs>> States => staSubject;

        public virtual bool IsMoving { get; }

        public MobileObject()
        {
            ViewSight = new Size();
            Position = new Point();
            Speed = 2;
            Angle = 0;

            Name = "Mobile object";

            StateEvent = new StateEvent();
            _stateQueue = new Queue<IState>();
            PointList = new List<Point>();
            State = new Standing();

            _disposables.Add(Observable.FromEventPattern<StateHandler, StateEventArgs>(
                ev => StateEvent.NextState += ev,
                ev => StateEvent.NextState -= ev).Subscribe(staSubject));

            _disposables.Insert(0, staSubject.Subscribe(x =>
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


            }));

            _disposables.Insert(0, Game.Intervals.CombineLatest(States, (tick, state) => state).Subscribe(x =>
            {
                if (State != null && CheckForUnExpected()) 
                    State.Act();
            }));
        }

        public virtual void EnqueueNextState()
        {
            _stateQueue.Enqueue(new Standing());
        }

        public virtual bool CheckForUnExpected()
        {
            return true;
        }

        private List<PointWithDistance> _visibleCells;
        public IReadOnlyList<PointWithDistance> VisibleCells
        {
            get
            {
                if (_visibleCells == null || !_visibleCells.Any() || _visibleCells.First().Point != PositionCell)
                {
                    _visibleCells = ShadowCasting.For(PositionCell, ViewRadius, Game.Map)
                        .GetVisibleCells()
                        .OrderBy(p => p.Distance)
                        .ToList();
                    _visibleCells.Insert(0, new PointWithDistance { Distance = 0, Point = PositionCell });
                }

                return _visibleCells;
            }
        }

        public Point DrawPosition
        {
            get { return _drawPosition; }
        }

        public Vector GetNearestPassibleVector(Vector currentVector)
        {
            if (IsVectorPassable(currentVector)) return currentVector;

            for (int i = 5; i <= 90; i+=5)
            {
                var antiClockVector = currentVector.TurnByAngle(i).Normalize();
                if (IsVectorPassable(antiClockVector)) return antiClockVector;

                var clockVector = currentVector.TurnByAngle(-i).Normalize();
                if (IsVectorPassable(clockVector)) return clockVector;
            }

            return currentVector;
        }

        private bool IsVectorPassable(Vector vector)
        {
            var destPoint = Position + new Point((int) (vector.X * ViewRadius*Map.CellMeasure), (int) (vector.Y * ViewRadius*Map.CellMeasure));
            var destCell = Map.PointToCell(destPoint);

            return VisibleCells.Any(vc => destCell == vc.Point && (Game.Map.GetObjectFromCell(vc)?.IsPassable ?? true));
        }
    }
}
