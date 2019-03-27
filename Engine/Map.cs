using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Objects.LargeObjects;
using Wintellect.PowerCollections;

namespace Engine
{
    public class Map: InnerMap
    {
        public const int CellMeasure = 20;
        public const int MAP_WIDTH = 1000;
        public const int MAP_HEIGHT = 1000;
        private const int MAP_CELL_WIDTH = MAP_WIDTH / CellMeasure;
        private const int MAP_CELL_HEIGHT = MAP_HEIGHT / CellMeasure;

        private InnerMap _innerMap;
        private Point _innerMapPoint;

       // private readonly FixedObject[,] _map;

        private List<MobileObject> _mobileObjects;

        public Rect VisibleRect { get; private set; }

        public Map(Rect rect, int sizeX = MAP_CELL_WIDTH, int sizeY = MAP_CELL_HEIGHT):base(sizeX, sizeY)
        {
            VisibleRect = rect;
            //_map = new FixedObject[sizeX, sizeY];
            _mobileObjects = new List<MobileObject>();
        }

        public void SetInnerMap(InnerMap innerMap, Point innerPoint)
        {
            _innerMap = innerMap;
            _innerMapPoint = innerPoint;
        }

        public void ClearInnerMap()
        {
            _innerMap = null;
            _innerMapPoint = null;
        }

        private int GetMapLength(int dimension)
        {
            return _map.GetLength(dimension)*CellMeasure;
        }
/*
        public FixedObject GetObjectFromDestination(Point destination)
        {
            var cell = PointToCell(destination);
            return this.GetObjectFromCell(cell);
        }
*/
        public FixedObject GetHRealObjectFromDestination(Point destination)
        {
            var cell = PointToCell(destination);
            var obj = GetHObjectFromCell(cell);

            if (obj is LargeObjectOuterAbstract largeObjectOuter)
                return largeObjectOuter.InnerObject;

            return obj;
        }

        private FixedObject GetHObjectFromCellXy(int x, int y)
        {
            if (CellInInnerMapXy(x, y))
            {
                return _innerMap.GetObjectFromCell(new Point(x - _innerMapPoint.X, y - _innerMapPoint.Y));
            }

            return _map[x, y]?.FixedObject;
        }

        public FixedObject GetHObjectFromCell(Point cell)
        {
            return GetHObjectFromCellXy(cell.X, cell.Y);
        }

        private void SetHObjectFromCell(Point cell, FixedObject gameObject)
        {
            if (CellInInnerMap(cell))
            {
                _innerMap.SetObjectFromCell(new Point(cell.X - _innerMapPoint.X, cell.Y - _innerMapPoint.Y), gameObject);
                return;
            }

            SetObjectFromCell(cell, gameObject);
        }

        private bool CellInInnerMapXy(int x, int y)
        {
            if (_innerMap != null && _innerMapPoint != null)
            {
                if (_innerMapPoint.X <= x && _innerMapPoint.X + _innerMap.GetSize().Width > x && _innerMapPoint.Y <= y &&
                    _innerMapPoint.Y + _innerMap.GetSize().Height > y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CellInInnerMap(Point cell)
        {
            return CellInInnerMapXy(cell.X, cell.Y);
        }

        internal void SetHObjectFromDestination(Point destination, FixedObject gameObject)
        {
            var cell = PointToCell(destination);
            SetHObjectFromCell(cell, gameObject);
        }

        internal static Point CellToPoint(Point point)
        {
            return new Point(point.X * CellMeasure, point.Y * CellMeasure);
        }

        internal static Point PointToCell(Point point)
        {
            return new Point(point.X / CellMeasure, point.Y / CellMeasure);
        }

        internal static Rect RectToCellRect(Rect rect)
        {
            return new Rect(rect.Left / CellMeasure, rect.Top / CellMeasure, rect.Width / CellMeasure, rect.Height / CellMeasure);
        }

        internal void AddMobileObject(MobileObject mobileObject)
        {
            _mobileObjects.Add(mobileObject);
        }

        public Rect GetInnerMapRect()
        {
            if (_innerMap == null)
            {
                return new Rect();
            }

            var size = _innerMap.GetSize();
            return new Rect(_innerMapPoint.X, _innerMapPoint.Y, size.Width, size.Height);
        }

        public Stack<Point> GetEasiestWay(Point start, Point dest)
        {
            var resultStack = new Stack<Point>();
            resultStack.Push(dest);

            var workPoints = new OrderedBag<WayPoint>();
            var processedPoints = new Dictionary<Point, WayPoint>();

            var startP = PointToCell(start);
            var destP = PointToCell(dest);

            if (startP.Equals(destP))
                return resultStack;

            var totdistX = Math.Abs(destP.X - startP.X);
            var totdistY = Math.Abs(destP.Y - startP.Y);
            var startWayP = new WayPoint(null, startP, 0, totdistX > totdistY ? totdistY * 14 + 10 * (totdistX - totdistY) : totdistX * 14 + 10 * (totdistY - totdistX));
            workPoints.Add(startWayP);
            processedPoints.Add(startWayP.Point, startWayP);
            WayPoint current = null;

            while (workPoints.Count > 0 /*&& !destP.Equals( workPoints.First().Point ) */)
            {
                current = workPoints.RemoveFirst();
                if (destP.Equals(current.Point))
                    break;

                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;

                        var temp = new Point
                        {
                            X = current.Point.X + dx,
                            Y = current.Point.Y + dy
                        };


                        if (temp.X < 0 || temp.X >= _map.GetLength(0))
                            continue;

                        if (temp.Y < 0 || temp.Y >= _map.GetLength(1))
                            continue;

                        if (GetHObjectFromCellXy(temp.X, temp.Y) != null && !GetHObjectFromCellXy(temp.X, temp.Y).IsPassable
                            && !destP.Equals(temp))
                            continue;

                        var tmpCost = ((dx + dy) % 2 == 0 ? 14 : 10) + current.Cost;
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
                            var distX = Math.Abs(destP.X - temp.X);
                            var distY = Math.Abs(destP.Y - temp.Y);
                            var tmpEvristic =
                                distX > distY ? distY * 14 + 10 * (distX - distY) : distX * 14 + 10 * (distY - distX);
                            var next = new WayPoint(current, temp, tmpCost, tmpEvristic);

                            workPoints.Add(next);
                            processedPoints.Add(next.Point, next);
                        }
                    }
                }

                current.IsProcessed = true;
            }

            current = current?.Parent;
            while (current != null)
            {
                var stackPoint = new Point(current.Point.X * CellMeasure + CellMeasure / 2, current.Point.Y * CellMeasure + CellMeasure / 2);

                if (!current.Point.Equals(destP) || GetHObjectFromCellXy(destP.X, destP.Y) == null || GetHObjectFromCellXy(destP.X, destP.Y).IsPassable)
                    resultStack.Push(stackPoint);

                current = current.Parent;
            }

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

            public Point Point { get; }
            public bool IsProcessed { get; set; }
            public int Cost { get; set; }
            private int Evristic { get; }

            //  protected WayPoint()

            public int CompareTo(WayPoint other)
            {
                if (ReferenceEquals(this, other)) return 0;

                return Cost + Evristic - other.Cost - other.Evristic;
                //  return Cost - other.Cost;
            }

            public override string ToString()
            {
                return $"Point: {Point}, Cost: {Cost}";
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
            x = x < 0 ? 0 : x + (int)VisibleRect.Width > GetMapLength(0) ? GetMapLength(0) - (int)VisibleRect.Width : x;

            var y = centerPosition.Y - VisibleRect.Height / 2;
            y = y < 0 ? 0 : y + VisibleRect.Height > GetMapLength(1) ? GetMapLength(1) - VisibleRect.Height : y;

            VisibleRect = new Rect(x, (int)y, VisibleRect.Width, VisibleRect.Height);
        }

        public List<MobileObject> GetMobileObjects()
        {
            return _mobileObjects;
        }

        public MobileObject GetMobileObject(Point cell)
        {
            return _mobileObjects.FirstOrDefault(mo => mo.PositionCell == cell);
        }

        public bool PointInVisibleRect(Point point)
        {
            return VisibleRect.Left <= point.X && VisibleRect.Left + VisibleRect.Width > point.X &&
                   VisibleRect.Top <= point.Y && VisibleRect.Top + VisibleRect.Height > point.Y;
        }

        public List<Point> GetNearestToPointList(Point positionPoint, int radius)
        {
            var cell = PointToCell(positionPoint);

            return GetNearestToCellList(cell, radius);
        }


        public Point GetNearestRandomEmptyCellFromPoint(Point positionPoint)
        {
            Point result = null;

            var neighbourList = Game.Map.GetNearestToPointList(positionPoint, 1);

            var positionCell = PointToCell(positionPoint);
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


        public void RemoveMobileObject(MobileObject mobileObject)
        {
            var drawPosition = mobileObject.DrawPosition;
            RemoveMobileObjectFromCell(drawPosition, mobileObject);
            _mobileObjects = _mobileObjects.Where(t => t != mobileObject).ToList();

        }

        public IReadOnlyList<MobileObject> GetMobileObjectsFromCell(Point drawCell)
        {
            return _map[drawCell.X, drawCell.Y]?.MobileList?.AsReadOnly();

        }
    }
}
