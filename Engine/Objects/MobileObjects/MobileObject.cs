using System;
using System.CodeDom;
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

        protected List<IDisposable> _disposables = new List<IDisposable>();

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

/*
        public Vector GetNearestVectorForFreeCell(Vector currentVector)
        {
            var div = 1.0;
            var isXMax = true;
            if (Math.Abs(currentVector.X) < Math.Abs(currentVector.Y))
            {
                isXMax = false;
                div = ViewRadius * Map.CellMeasure / currentVector.Y;
            }
            else
                div = ViewRadius * Map.CellMeasure / currentVector.X;

            var destPoint = Position + new Point((int)(currentVector.X* div), (int)(currentVector.Y*div));
            var destCell = Map.PointToCell(destPoint);

            if (NearestVectorForFreeCell(currentVector, destCell, isXMax, out var vector)) return vector;

            return currentVector;
        }

        private bool NearestVectorForFreeCell(Vector currentVector, Point destCell, bool isXMax, out Vector vector)
        {
            if (VisibleCells.Any(vc => destCell == vc.Point && (Game.Map.GetObjectFromCell(vc)?.IsPassable ?? true)))
            {
                {
                    vector = currentVector;
                    return true;
                }
            }

            for (int i = 1; i <= ViewRadius * 2; i++)
            {
                if (isXMax)
                {
                    var clockTuple = VectorToNearestEmptyCellForQuoter(Math.Sign(currentVector.X), PositionCell.Y, destCell.X,
                        destCell.Y, i, true);
                    var clockPoint = new Point(clockTuple.Item2, clockTuple.Item1);
                    if (IsCellFree(clockPoint, out var normalize))
                    {
                        vector = normalize;
                        return true;
                    }

                    var counterclockTuple = VectorToNearestEmptyCellForQuoter(Math.Sign(currentVector.X), PositionCell.Y,
                        destCell.X, destCell.Y, i, false);
                    var counterclockPoint = new Point(counterclockTuple.Item1, counterclockTuple.Item2);
                    if (IsCellFree(counterclockPoint, out var counterNormalize))
                    {
                        vector = counterNormalize;
                        return true;
                    }
                }
                else
                {
                    var clockTuple = VectorToNearestEmptyCellForQuoter(Math.Sign(currentVector.Y), PositionCell.X, destCell.Y,
                        destCell.X, i, true);
                    var clockPoint = new Point(clockTuple.Item1, clockTuple.Item2);
                    if (IsCellFree(clockPoint, out var normalize))
                    {
                        vector = normalize;
                        return true;
                    }

                    var counterclockTuple = VectorToNearestEmptyCellForQuoter(Math.Sign(currentVector.Y), PositionCell.X,
                        destCell.Y, destCell.X, i, false);
                    var counterclockPoint = new Point(counterclockTuple.Item1, counterclockTuple.Item2);
                    if (IsCellFree(counterclockPoint, out var counterNormalize))
                    {
                        vector = counterNormalize;
                        return true;
                    }

                    /*
                                        var isTurn2 = Math.Abs((destCell.X + Math.Sign(currentVector.Y) * i) - PositionCell.X) > ViewRadius;
                                        var x2 = isTurn2 ? PositionCell.X + Math.Sign(currentVector.Y) * ViewRadius : (destCell.X + Math.Sign(currentVector.Y) * i);
                                        var y2 = isTurn2 ? destCell.Y + Math.Sign(currentVector.Y) + (i - (PositionCell.X + Math.Sign(currentVector.Y) * ViewRadius)) : destCell.Y;
                                        var counterclockCell = new Point(x, y);
                    */
         /*       }
            }

            return false;
        }

        private bool IsCellFree(Point clockPoint, out Vector normalize)
        {
            if (VisibleCells.Any(vc => clockPoint == vc.Point && (Game.Map.GetObjectFromCell(vc)?.IsPassable ?? true)))
            {
                {
                    normalize = Vector.FromPoints(Position, Map.CellToPoint(clockPoint)).Normalize();
                    return true;
                }
            }

            normalize = null;
            return false;
        }

        private Tuple<int, int> VectorToNearestEmptyCellForQuoter(int startDirection, int centralPosition, int leadingStartPosition, int lessStartPosition, int i, bool clockDirection)
        {
            var clockKoeff = clockDirection ? -1 : 1;
            var isTurn = Math.Abs((lessStartPosition + clockKoeff * startDirection * i) - centralPosition) > ViewRadius;

            var x = isTurn
                ? centralPosition + clockKoeff * startDirection * ViewRadius
                : (lessStartPosition + clockKoeff * startDirection * i);

            var y = isTurn
                ? leadingStartPosition + startDirection * (i - Math.Abs(centralPosition + clockKoeff * startDirection * ViewRadius - lessStartPosition))
                : leadingStartPosition;

            return new Tuple<int, int>(x, y);
        }*/
    }
}
