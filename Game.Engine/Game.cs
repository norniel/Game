using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using Game.Engine.BridgeObjects;
using Game.Engine.Interfaces;
using Wintellect.PowerCollections;
using Game.Engine.Objects;

namespace Game.Engine
{
    public class Game
    {
        private const uint CELL_MASK = 0x11000000;
        private const uint OBJ_MASK = 0x00111100;

        private const int CELL_MEASURE = 20;

        readonly uint[,] _map;
        Rect curRect;

        Dictionary< uint, Cell> _cellSamples;
        Dictionary< uint, FixedObject > _objectSamples;

        private LoadSaveManager loadSaveManager;

        private readonly Timer _timer;

        private readonly Hero _hero;

        private readonly IDrawer _drawer;

        private IActionRepository ActionRepository { get; set; }
    
        public Game( IDrawer drawer, uint width, uint height )
        {
            curRect.Width = width;
            curRect.Height = height;
            _map = new uint[curRect.Width / CELL_MEASURE, curRect.Height / CELL_MEASURE];

            loadSaveManager = new LoadSaveManager();
            loadSaveManager.LoadSnapshot( _map );

            _objectSamples = new Dictionary<uint, FixedObject>();
            _objectSamples[0x00000000] = new FixedObject(); // automize
            _objectSamples[0x00000100] = new Tree(); // automize
            _objectSamples[0x00001100] = new Plant(); // automize

            _timer = new Timer(100);
            _timer.Enabled = true;
            _timer.Elapsed += OnTimedEvent;

            _hero = new Hero();

            ActionRepository = new ActionRepository();

            _drawer = drawer;
            //_drawer.GetAction = (x, y) => GetActions(new Point(x, y));
        }


        private void LoadSettings()
        {
            Properties.Settings.Default.Reload();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _hero.State.Act();
        }

        public void LClick( Point destination )
        {
            MoveToDest(destination);
        }
        
        public void RClick(Point destination)
        {
           ShowActions(destination);
        }

        private void ShowActions(Point destination)
        {
            this._drawer.DrawMenu(destination.X, destination.Y, GetActions(destination));
        }
        
        private List<ClientAction> GetActions(Point destination)
        {
            var destCell = PointToCell(destination);

            if (_map[destCell.X, destCell.Y] == 0)
            {
                return new List<ClientAction> {
                    new ClientAction
                    {
                        Name = "Go",
                        CanDo = true,
                        Do = () => { }
                    }
                };
            }

            //return new List<string> { _objectSamples[_map[destCell.X, destCell.Y]] .GetType().FullName};

            var gameObject = _objectSamples[_map[destCell.X, destCell.Y]];

            var possibleActions = ActionRepository.GetPossibleActions(gameObject).ToList();

            var objects = _hero.GetContainerItems().Union(new[]{gameObject}).ToList();
            return possibleActions.Select(pa => new ClientAction
            {
                Name = pa.Name,
                CanDo = pa.CanDo(_hero, objects),
                Do = () => pa.Do(_hero, objects)
            }).ToList();

        }

        public void MoveToDest( Point destination )
        {
            _hero.StartMove( destination, GetEasiestWay( _hero.Position, destination ) );
        }

        class WayPoint : IComparable<WayPoint>
        {
            public WayPoint(WayPoint parent, Point point, int cost, int evristic )
            {
                Point = new Point( point );
                Cost = cost;
                Parent = parent;
                IsProcessed = false;
                Evristic = evristic;
            }

            public WayPoint Parent { get; set; }

            public Point Point { get; set; }
            public bool IsProcessed { get; set; }
            public int Cost { get; set; }
            public int Evristic { get; private set;  }

          //  protected WayPoint()

            public int CompareTo( WayPoint other )
            {
                if (ReferenceEquals(this, other)) return 0;

                return Cost + Evristic - other.Cost - other.Evristic;
              //  return Cost - other.Cost;
            }

            public override string ToString()
            {
                return string.Format("Point: {0}, Cost: {1}", Point, Cost);
            }
        }



        public Stack<Point> GetEasiestWay(Point start, Point dest)
        {
            Stack<Point> resultStack = new Stack<Point>();
            resultStack.Push(dest);

            OrderedBag<WayPoint> workPoints = new OrderedBag<WayPoint>();
            Dictionary<Point, WayPoint> processedPoints = new Dictionary<Point, WayPoint>();

            Point startP = PointToCell(start);
            Point destP = PointToCell(dest);

            if (startP.Equals(destP))
                return resultStack;

            int totdistX = Math.Abs(destP.X - startP.X);
            int totdistY = Math.Abs(destP.Y - startP.Y);
            WayPoint startWayP = new WayPoint(null, startP, 0, (totdistX > totdistY ? totdistY * 14 + 10 * (totdistX - totdistY) : totdistX * 14 + 10 * (totdistY - totdistX)));
            workPoints.Add(startWayP);
            processedPoints.Add(startWayP.Point, startWayP);
            WayPoint current = null;

            while (workPoints.Count > 0 /*&& !destP.Equals( workPoints.First().Point ) */)
            {
                current = workPoints.RemoveFirst();
                if (destP.Equals(current.Point))
                    break;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        Point temp = new Point();
                        if (dx == 0 && dy == 0)
                            continue;
                        temp.X = (int) current.Point.X + dx;
                        if (temp.X < 0 || temp.X >= _map.GetLength(0))
                            continue;

                        temp.Y = (int) current.Point.Y + dy;
                        if (temp.Y < 0 || temp.Y >= _map.GetLength(1))
                            continue;

                        if (!_objectSamples[_map[temp.X, temp.Y] & OBJ_MASK].IsPassable)
                            continue;

                        int tmpCost = (((dx + dy)%2 == 0) ? 14 : 10) + current.Cost;
                            // если обрабатываемая клетка лежит по диагонали к родительской - прибавляем 14(приближ. корень2), если нет - 10

                        if (processedPoints.ContainsKey(temp))
                        {
                            if (processedPoints[temp].IsProcessed)
                                continue;

                            if (tmpCost < processedPoints[temp].Cost)
                            {
                                processedPoints[temp].Cost = tmpCost;
                                processedPoints[temp].Parent = current;
                            }
                        }
                        else
                        {
                            int distX = (int) Math.Abs((int) destP.X - (int) temp.X);
                            int distY = (int) Math.Abs((int) destP.Y - (int) temp.Y);
                            int tmpEvristic =
                                (int) (distX > distY ? distY*14 + 10*(distX - distY) : distX*14 + 10*(distY - distX));
                            WayPoint next = new WayPoint(current, temp, tmpCost, tmpEvristic);

                            workPoints.Add(next);
                            processedPoints.Add(next.Point, next);
                        }
                    }
                }

                current.IsProcessed = true;


            }

            if( workPoints.Count == 0 )
            {
                resultStack.Clear();
              //  current = processedPoints.Values.Min()
              //  current = processedPoints.Values.OrderBy( waypoint => waypoint.Evristic).First();
            }


          //  if( workPoints.Count > 0 )
          //  {
                Point stackPoint;
                while( current != null )
                {
                    stackPoint = new Point(current.Point.X * CELL_MEASURE  + CELL_MEASURE/2, current.Point.Y * CELL_MEASURE  + CELL_MEASURE/2);
                    resultStack.Push( stackPoint );
                    current = current.Parent;
                }
          //  }

            return resultStack;
        }

        private static Point PointToCell(Point point)
        {
            return new Point(point.X/CELL_MEASURE, point.Y/CELL_MEASURE);
        }

        public void DrawChanges()
        {
            _drawer.Clear();
            
            _drawer.DrawSurface(curRect.Width, curRect.Height);
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    _drawer.DrawObject(_objectSamples[_map[i, j] & OBJ_MASK].Id, i * CELL_MEASURE, j * CELL_MEASURE);
                }
            }

            _drawer.DrawHero( _hero.Position, _hero.Angle, _hero.PointList );

            _drawer.DrawContainer(_hero.GetContainerItems().Select(go => go.Name));
        }

    }
}
