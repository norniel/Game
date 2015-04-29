using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine.Interfaces;
using Game.Engine.Objects.LargeObjects;
using Microsoft.Practices.ObjectBuilder2;
using Wintellect.PowerCollections;

namespace Game.Engine
{
    public class Map: IMap
    {
        private const int CELL_MEASURE = 20;
        private const int MAP_WIDTH = 1000;
        private const int MAP_HEIGHT = 1000;
        private const int MAP_CELL_WIDTH = MAP_WIDTH / CELL_MEASURE;
        private const int MAP_CELL_HEIGHT = MAP_HEIGHT / CELL_MEASURE;

        private readonly FixedObject[,] _map;

        private readonly List<MobileObject> _mobileObjects;

        private Rect _visibleRect;
        public Rect VisibleRect {
            get { return _visibleRect; }
            set { _visibleRect = value; }
        }

        public Map(Rect rect)
        {
            VisibleRect = rect;
            _map = new FixedObject[MAP_CELL_WIDTH, MAP_CELL_HEIGHT];
            _mobileObjects = new List<MobileObject>();
            //var sizeInCells = RectToCellRect(rect);
            //_map = new FixedObject[sizeInCells.Width, sizeInCells.Height];
        }

        public FixedObject GetObjectFromDestination(Point destination)
        {
            var cell = PointToCell(destination);
            return this.GetObjectFromCell(cell);
        }

        public FixedObject GetRealObjectFromDestination(Point destination)
        {
            var cell = PointToCell(destination);
            var obj = this.GetObjectFromCell(cell);

            var largeObjectOuter = obj as LargeObjectOuterAbstract;

            if (largeObjectOuter != null)
                return largeObjectOuter.InnerObject;

            return obj;
        }

        public FixedObject GetObjectFromCell(Point cell)
        {
            return _map[cell.X, cell.Y];
        }

        internal FixedObject GetObjectFromCellXY(int x, int y)
        {
            return _map[x, y];
        }

        internal void SetObjectFromDestination(Point destination, FixedObject gameObject)
        {
            var cell = PointToCell(destination);
            this.SetObjectFromCell(cell, gameObject);
        }

        internal void SetObjectFromCell(Point cell, FixedObject gameObject)
        {
            if (cell == null)
                return;

            if (gameObject == null)
            {
                _map[cell.X, cell.Y] = null;
                return;
            }

            var largeObjectInner = gameObject as LargeObjectInner;

            if (largeObjectInner != null)
            {
                largeObjectInner.OuterObjects.ForEach(outerObject =>
                {
                    SetObjectFromCell(new Point(cell.X + outerObject.PlaceInObject.X, cell.Y + outerObject.PlaceInObject.Y), outerObject);
                });

                return;
            }

            if (gameObject.RemoveFromContainer != null)
            {
                gameObject.RemoveFromContainer();
            }

            gameObject.RemoveFromContainer = (() =>
            {
                this.SetObjectFromCell(cell, null);

                if (gameObject.Properties.Contains(Property.Regrowable) && gameObject is ICloneable)
                {
                    this.SetObjectFromCell(this.GetRandomNearEmptyPoint(cell, 3), (FixedObject)(gameObject as ICloneable).Clone());
                }
            });

            if (gameObject.Properties.Contains(Property.Dropable))
            {
                gameObject.Properties.Remove(Property.Dropable);

                if (!gameObject.Properties.Contains(Property.Pickable))
                    gameObject.Properties.Add(Property.Pickable);
            }

            _map[cell.X, cell.Y] = gameObject;
        }

        private Point GetRandomNearEmptyPoint(Point cell, int radius)
        {
            var nearestPoints = GetNearestToCellList(cell, radius);

            var random = new Random();

            while (nearestPoints.Any())
            {
                var p = random.Next(nearestPoints.Count);
                if (this.GetObjectFromCell(nearestPoints[p]) == null)
                {
                    return nearestPoints[p];
                }

                nearestPoints.RemoveAt(p);
            }

            return null;
        }

        internal static Point CellToPoint(Point point)
        {
            return new Point(point.X * CELL_MEASURE, point.Y * CELL_MEASURE);
        }

        internal static Point PointToCell(Point point)
        {
            return new Point(point.X / CELL_MEASURE, point.Y / CELL_MEASURE);
        }

        public Rect GetSize()
        {
            return new Rect(0, 0, (uint)_map.GetLength(0), (uint)_map.GetLength(1));
        }

        internal static Rect RectToCellRect(Rect rect)
        {
            return new Rect(rect.Left / CELL_MEASURE, rect.Top / CELL_MEASURE, rect.Width / CELL_MEASURE, rect.Height / CELL_MEASURE);
        }

        internal void AddMobileObject(MobileObject mobileObject)
        {
            _mobileObjects.Add(mobileObject);
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
                        temp.X = (int)current.Point.X + dx;
                        if (temp.X < 0 || temp.X >= _map.GetLength(0))
                            continue;

                        temp.Y = (int)current.Point.Y + dy;
                        if (temp.Y < 0 || temp.Y >= _map.GetLength(1))
                            continue;

                        if (_map[temp.X, temp.Y] != null && !_map[temp.X, temp.Y].IsPassable
                            && !destP.Equals(temp))
                            continue;

                        int tmpCost = (((dx + dy) % 2 == 0) ? 14 : 10) + current.Cost;
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
                            int distX = (int)Math.Abs((int)destP.X - (int)temp.X);
                            int distY = (int)Math.Abs((int)destP.Y - (int)temp.Y);
                            int tmpEvristic =
                                (int)(distX > distY ? distY * 14 + 10 * (distX - distY) : distX * 14 + 10 * (distY - distX));
                            WayPoint next = new WayPoint(current, temp, tmpCost, tmpEvristic);

                            workPoints.Add(next);
                            processedPoints.Add(next.Point, next);
                        }
                    }
                }

                current.IsProcessed = true;
            }

            if (workPoints.Count == 0)
            {
                resultStack.Clear();
                //  current = processedPoints.Values.Min()
                //  current = processedPoints.Values.OrderBy( waypoint => waypoint.Evristic).First();
            }


            //  if( workPoints.Count > 0 )
            //  {
            Point stackPoint;
            while (current != null)
            {
                stackPoint = new Point(current.Point.X * CELL_MEASURE + CELL_MEASURE / 2, current.Point.Y * CELL_MEASURE + CELL_MEASURE / 2);

                if (!current.Point.Equals(destP) || _map[destP.X, destP.Y] == null || _map[destP.X, destP.Y].IsPassable)
                    resultStack.Push(stackPoint);

                current = current.Parent;
            }
            //  }

            return resultStack;
        }

        private class WayPoint : IComparable<WayPoint>
        {
            public WayPoint(WayPoint parent, Point point, int cost, int evristic)
            {
                Point = new Point(point);
                Cost = cost;
                Parent = parent;
                IsProcessed = false;
                Evristic = evristic;
            }

            public WayPoint Parent { get; set; }

            public Point Point { get; set; }
            public bool IsProcessed { get; set; }
            public int Cost { get; set; }
            public int Evristic { get; private set; }

            //  protected WayPoint()

            public int CompareTo(WayPoint other)
            {
                if (ReferenceEquals(this, other)) return 0;

                return Cost + Evristic - other.Cost - other.Evristic;
                //  return Cost - other.Cost;
            }

            public override string ToString()
            {
                return String.Format("Point: {0}, Cost: {1}", Point, Cost);
            }
        }

        public Point GetRealDestinationFromVisibleDestination(Point visibleDestination)
        {
            return new Point(visibleDestination.X + VisibleRect.Left, visibleDestination.Y + VisibleRect.Top);
        }

        public Point GetVisibleDestinationFromRealDestination(Point realDestination)
        {
            return new Point(realDestination.X - VisibleRect.Left, realDestination.Y - VisibleRect.Top);
        }

        public void RecalcVisibleRect(Point centerPosition)
        {
            var x = centerPosition.X - (int)(VisibleRect.Width/2);
            x = x < 0 ? 0 : (x + (int)VisibleRect.Width > MAP_WIDTH ? MAP_WIDTH - (int)VisibleRect.Width : x);

            var y = centerPosition.Y - VisibleRect.Height / 2;
            y = y < 0 ? 0 : (y + VisibleRect.Height > MAP_HEIGHT ? MAP_HEIGHT - VisibleRect.Height : y);

            VisibleRect = new Rect(x, (int)y, VisibleRect.Width, VisibleRect.Height);
        }

        public List<MobileObject> GetMobileObjects()
        {
            return _mobileObjects;
        }

        public bool PointInVisibleRect(Point point)
        {
            return VisibleRect.Left <= point.X && (VisibleRect.Left + VisibleRect.Width) > point.X &&
                   VisibleRect.Top <= point.Y && (VisibleRect.Top + VisibleRect.Height) > point.Y;
        }

        public List<Point> GetNearestToPointList(Point positionPoint, int radius)
        {
            var cell = PointToCell(positionPoint);

            return GetNearestToCellList(cell, radius);
        }

        public List<Point> GetNearestToCellList(Point positionCell, int radius)
        {
            var nearestPoints = new List<Point>();

            for (int i = positionCell.X - radius < 0 ? 0 : positionCell.X - radius; i <= (positionCell.X + radius >= MAP_CELL_WIDTH ? MAP_CELL_WIDTH - 1 : positionCell.X + radius); i++)
            {
                for (int j = positionCell.Y - radius < 0 ? 0 : positionCell.Y - radius; j <= (positionCell.Y + radius >= MAP_CELL_HEIGHT ? MAP_CELL_HEIGHT - 1 : positionCell.Y + radius); j++)
                {
                    nearestPoints.Add(new Point(i, j));
                }
            }

            return nearestPoints;
        }

        public Point GetNearestRandomEmptyCellFromPoint(Point positionPoint)
        {
            Point result = null;

            var neighbourList = Game.Map.GetNearestToPointList(positionPoint, 1);

            var positionCell = Map.PointToCell(positionPoint);
            neighbourList.Remove(positionCell);

            while (neighbourList.Any())
            {
                var p = Game.Random.Next(neighbourList.Count);

                var obj = Game.Map.GetObjectFromCell(neighbourList[p]);
                if (obj != null && !obj.IsPassable)
                {
                    neighbourList.RemoveAt(p);
                    continue;
                }

                result = neighbourList[p];
                break;
            }

            return result;
        }


    }
}
