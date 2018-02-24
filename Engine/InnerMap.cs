using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Interfaces;
using Engine.Objects.LargeObjects;

namespace Engine
{
    public class InnerMap:IMap
    {
        protected readonly FixedObject[,] _map;
        
        public InnerMap(int sizeX, int sizeY)
        {
            _map = new FixedObject[sizeX, sizeY];
        }
        
        public Rect GetSize()
        {
            return new Rect(0, 0, (uint)_map.GetLength(0), (uint)_map.GetLength(1));
        }

        public FixedObject GetObjectFromCell(Point cell)
        {
            return GetObjectFromCellXY(cell.X, cell.Y);
        }

        protected virtual FixedObject GetObjectFromCellXY(int x, int y)
        {
            return _map[x, y];
        }

        internal void SetObjectFromCell(Point cell, FixedObject gameObject)
        {
            if (cell == null)
            {
                return;
            }

            if (gameObject == null)
            {
                _map[cell.X, cell.Y] = null;
                return;
            }

            var largeObjectInner = gameObject as LargeObjectInner;

            if (largeObjectInner != null)
            {
                foreach(var outerObject in largeObjectInner.OuterObjects)
                {
                    var outerO = outerObject;
                    SetObjectFromCell(new Point(cell.X + outerO.PlaceInObject.X, cell.Y + outerO.PlaceInObject.Y), outerO);
                }

                return;
            }

            if (gameObject.RemoveFromContainer != null)
            {
                gameObject.RemoveFromContainer();
            }

            gameObject.RemoveFromContainer = (() =>
            {
                gameObject.RemoveFromContainer = null;

                if (gameObject == _map[cell.X, cell.Y])
                {
                    SetObjectFromCell(cell, null);
                }

                if (gameObject.Properties.Contains(Property.Regrowable) && gameObject is ICloneable)
                {
                    SetObjectFromCell(GetRandomNearEmptyPoint(cell, 3), (FixedObject)(gameObject as ICloneable).Clone());
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
                if (GetObjectFromCell(nearestPoints[p]) == null)
                {
                    return nearestPoints[p];
                }

                nearestPoints.RemoveAt(p);
            }

            return null;
        }

        public List<Point> GetNearestToCellList(Point positionCell, int radius)
        {
            var nearestPoints = new List<Point>();

            for (int i = positionCell.X - radius < 0 ? 0 : positionCell.X - radius; i <= (positionCell.X + radius >= _map.GetLength(0) ? _map.GetLength(0) - 1 : positionCell.X + radius); i++)
            {
                for (int j = positionCell.Y - radius < 0 ? 0 : positionCell.Y - radius; j <= (positionCell.Y + radius >= _map.GetLength(1) ? _map.GetLength(1) - 1 : positionCell.Y + radius); j++)
                {
                    nearestPoints.Add(new Point(i, j));
                }
            }

            return nearestPoints;
        }
    }
}
