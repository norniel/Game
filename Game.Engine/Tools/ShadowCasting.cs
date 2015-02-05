using System.Collections.Generic;
using System.Linq;

namespace Game.Engine.Tools
{
    class ShadowCasting
    {
        private readonly Point _startingCell;
        private readonly int _radius;
        private readonly Map _map;
        private readonly int _mapWidth;
        private readonly int _mapHeight;

        public ShadowCasting(Point startingCell, int radius, Map map)
        {
            _startingCell = startingCell;
            _radius = radius;
            _map = map;
            _mapWidth = (int)_map.GetSize().Width;
            _mapHeight = (int)_map.GetSize().Height;
        }

        public Point GetFirstCell()
        {
            return null;
        }

        public List<PointWithDistance> GetVisibleCells()
        {
            IEnumerable<PointWithDistance> result = null;

            for (int i = 0; i < 4; i++)
            {
                result = i == 0 ? GetVisibleCellsFromQuoter(i) : result.Concat(GetVisibleCellsFromQuoter(i));
            }

            return result.ToList();
        }

        private IEnumerable<PointWithDistance> GetVisibleCellsFromQuoter(int i)
        {
            return CalculateVisibleCellsFromQuoter(i, -1, 1, 1);
        }

        private IEnumerable<PointWithDistance> CalculateVisibleCellsFromQuoter(int quoterNumber, decimal startingSlope, decimal endingSlope, int depth)
        {
            for (int j = depth; j <= _radius; j++)
            {
                if (ActualYOutOfRange(j, quoterNumber))
                    break;

                var startingI = (int)(startingSlope * j);
                var endingI = (int)(endingSlope * j);
                // 0) j = _startingCell.Y - y; i = x - _startingCell.X ;
                // 1) j = _startingCell.X - x; i = _startingCell.Y - y;
                // 2) j = y - _startingCell.Y; i = _startingCell.X - x;
                // 3) j = x - _startingCell.X; i = y -_startingCell.Y;
                //var startingX = (int)(startingSlope*(y - _startingCell.Y) + _startingCell.X);
                //var endingX = (int)(endingSlope * (y - _startingCell.Y) + _startingCell.X);

                var iBound = ReturnIBound(quoterNumber, startingI, endingI);
                startingI = iBound.X;
                endingI = iBound.Y;

                for (int i = startingI; i <= endingI; i++)
                {
                    if (GetObjectFromCellInQuoter(quoterNumber, i, j).IsPassable && i < endingI && !GetObjectFromCellInQuoter(quoterNumber, i + 1, j).IsPassable)
                    {
                        CalculateVisibleCellsFromQuoter(quoterNumber, startingSlope, GetNewSlope(i - 0.5m, j + ((i < 0) ? 0.5m : -0.5m)), j);
                    }
                    else if (!GetObjectFromCellInQuoter(quoterNumber, i, j).IsPassable && i < endingI && GetObjectFromCellInQuoter(quoterNumber, i + 1, j).IsPassable)
                    {
                        startingSlope = GetNewSlope(i + 0.5m, j - ((i < 0) ? 0.5m : -0.5m));
                    }

                    yield return new PointWithDistance() { Distance = 0, Point = GetCellFromIJInQuoter(quoterNumber, i, j) };
                }

                if (!GetObjectFromCellInQuoter(quoterNumber, endingI, j).IsPassable)
                    break;
            }
        }

        private decimal GetNewSlope(decimal i, decimal j)
        {
            return i/j;
        }

        private bool ActualYOutOfRange(int j, int quoter)
        {
            return ((quoter == 0 && _startingCell.Y - j < 0) || 
                (quoter == 1 && _startingCell.X - j < 0) ||
                (quoter == 2 && _startingCell.Y + j >= _mapHeight) || 
                (quoter == 3 && _startingCell.X + j >= _mapWidth));
        }

        private Point ReturnIBound(int quoter, int startingI, int endingI)
        {
            var result = new Point();
            switch (quoter)
            {
                case 0:
                    result.X = _startingCell.X + startingI < 0 ? -_startingCell.X : startingI;
                    result.Y = _startingCell.X + endingI >= _mapWidth ? _mapWidth - 1 - _startingCell.X : endingI;
                    break;
                case 1:
                    result.X = _startingCell.Y - startingI >= _mapHeight ? _startingCell.Y - _mapHeight + 1 : startingI;
                    result.Y = _startingCell.Y - endingI < 0 ? _startingCell.Y : endingI;
                    break;
                case 2:
                    result.X = _startingCell.X - startingI >= _mapWidth ? _startingCell.X - _mapWidth + 1 : startingI;
                    result.Y = _startingCell.X - endingI < 0 ? _startingCell.X : endingI;
                    break;
                case 3:
                    result.X = _startingCell.Y + startingI < 0 ? -_startingCell.Y : startingI;
                    result.Y = _startingCell.Y + endingI >= _mapHeight ? _mapHeight - 1 - _startingCell.Y : endingI;
                    break;
            }
            
            return result;
        }

        private Point GetCellFromIJInQuoter(int quoter, int i, int j)
        {
            switch (quoter)
            {
                case 0:
                    return new Point(_startingCell.X + i, _startingCell.Y - j);
                case 1:
                    return new Point(_startingCell.X - j, _startingCell.Y - i);
                case 2:
                    return new Point(_startingCell.X - i, _startingCell.Y + j);
                case 3:
                    return new Point(_startingCell.X + j, _startingCell.Y + i);
            }

            return null;
        }

        private FixedObject GetObjectFromCellInQuoter(int quoter, int i, int j)
        {
            return _map.GetObjectFromCell(GetCellFromIJInQuoter(quoter, i, j));
        }
    }

    internal class PointWithDistance
    {
        public Point Point { get; set; }
        public int Distance { get; set; }
    }
}
