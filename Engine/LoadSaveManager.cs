using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Objects.Animals;

namespace Engine
{
    class LoadSaveManager
    {
        internal void LoadSnapshot( Map map )
        {
            GenerateMap( map );
        }

        internal void SaveSnapshot()
        { }

        internal void SaveChanges()
        { }

        internal void LoadHero()
        { }

        internal void SaveHero()
        {}

        private void GenerateMap(Map map)
        {
            var mapSize = map.GetSize();
            int width = (int)mapSize.Width - 1;
            int height = (int)mapSize.Height - 1;

            int count = (int)(width*height*0.35);

            var mapWithRiver = GenerateRiver(map);

            for (int i = 0; i < mapWithRiver.GetLength(0); i++)
            {
                for (int j = 0; j < mapWithRiver.GetLength(1); j++)
                {
                    if (mapWithRiver[i, j] > 0)
                        map.SetObjectFromCell(new Point(i, j), new FixedObject(new Size(1, 1), (uint)(mapWithRiver[i, j] > 1 ? 0x00002000 : 0x00002100)){IsPassable = false});
                }
            }
            
            int tmpX, tmpY;
            Random rand = new Random();
            
            while( count > 0 )
            {
                tmpX = rand.Next(width);
                tmpY = rand.Next(height);

                if( map.GetObjectFromCell(new Point(tmpX, tmpY)) != null )
                    continue;

                var typesCount = Game.Factory.ObjectsToGenMap.Count;
                /*
                var typesOnMap = Assembly.GetExecutingAssembly().GetTypes().Where(
                    type => type.GetCustomAttribute<GenerateMapAttribute>() != null).ToList();
                */
                var randIndex = count % typesCount;//typesOnMap.Count;

                //map.SetObjectFromCell(new Point(tmpX, tmpY), Activator.CreateInstance(typesOnMap[randIndex]) as FixedObject);
                map.SetObjectFromCell(new Point(tmpX, tmpY), Game.Factory.Produce(Game.Factory.ObjectsToGenMap[randIndex]) as FixedObject);
                
                count--;
            }

            for (int i = 0; i < 20; i++)
            {
                while (true)
                {
                    tmpX = rand.Next(width);
                    tmpY = rand.Next(height);

                    if (map.GetObjectFromCell(new Point(tmpX, tmpY)) != null)
                        continue;

             //       map.AddMobileObject(new Dikabryozik(new Point(tmpX, tmpY)));
                    map.AddMobileObject(new Hare(Map.CellToPoint(new Point(tmpX, tmpY))));
                    map.AddMobileObject(new Fox(Map.CellToPoint(new Point(tmpX, tmpY))));
                    break;
                }
            }

        }

        private int[,] GenerateRiver(Map map)
        {
            var mapSize = map.GetSize();
            int width = (int)mapSize.Width - 1;
            int height = (int)mapSize.Height - 1;
            var mapTmp = new int[mapSize.Width, mapSize.Height];
            int river_length = 40;
            int deep = 2;
            var pointList = new List<Point>();

            int tmpX, tmpY;
            Random rand = new Random();
            tmpX = rand.Next(width);
            tmpY = rand.Next(height);
            tmpX = tmpX <= 1 ? tmpX + 10 : tmpX >= width - 1 ? tmpX - 10 : tmpX;
            tmpY = tmpY <= 1 ? tmpY + 10 : tmpY >= height - 1 ? tmpY - 10 : tmpY;
            mapTmp[tmpX, tmpY] = deep;
            pointList.Add(new Point(tmpX, tmpY));

            var possibility = rand.Next(4);
            tmpX = possibility == 0 ? tmpX - 1 : possibility == 1 ? tmpX + 1 : tmpX;
            tmpY = possibility == 2 ? tmpY - 1 : possibility == 3 ? tmpY + 1 : tmpY;
            mapTmp[tmpX, tmpY] = deep;
            pointList.Add(new Point(tmpX, tmpY));

            while (pointList.Count < river_length)
            {
                var currentListLength = pointList.Count;
                var curPoint = pointList.First();
                var firstPoint = AddDeepPointToRiver(curPoint, width, height, mapTmp, deep, rand);

                if(!ReferenceEquals(firstPoint, null))
                    pointList.Insert(0, firstPoint);

                curPoint = pointList.Last();
                var lastPoint = AddDeepPointToRiver(curPoint, width, height, mapTmp, deep, rand);

                if (!ReferenceEquals(lastPoint, null))
                    pointList.Add(lastPoint);

                if(currentListLength == pointList.Count)
                    break;
            }
            
            while (pointList.Count > 0)
            {
                var curPoint = pointList.First();
                pointList.RemoveAt(0);

                var curDeepness = mapTmp[curPoint.X, curPoint.Y];

                if(curDeepness <= 0)
                    continue;

                for (int i = curPoint.X - 1; i <= curPoint.X + 1; i++)
                {
                    for (int j = curPoint.Y - 1; j <= curPoint.Y + 1; j++)
                    {
                        if ((i == curPoint.X && j == curPoint.Y) || i < 0 || i > width || j < 0 || j > height
                            || mapTmp[i, j] > 0)
                            continue;

                        var possibilityForDeepness = rand.Next(9);

                        var nextDeepness = possibilityForDeepness < 8 ? curDeepness - 1 : curDeepness;

                        if(nextDeepness <= 0)
                            continue;

                        mapTmp[i, j] = nextDeepness;
                        pointList.Add(new Point(i, j));
                    }
                }
            }
            return mapTmp;
        }

        private Point AddDeepPointToRiver(Point curPoint, int width, int height, int[,] mapTmp, int deep, Random rand)
        {
            if (curPoint.X > 0 && curPoint.X < width && curPoint.Y > 0 && curPoint.Y < height)
            {
                var tmpList = new List<Point>();

                AddPointToTmpListIfNotInRiver(mapTmp, curPoint, deep, tmpList, -1, 0);
                AddPointToTmpListIfNotInRiver(mapTmp, curPoint, deep, tmpList, 1, 0);
                AddPointToTmpListIfNotInRiver(mapTmp, curPoint, deep, tmpList, 0, -1);
                AddPointToTmpListIfNotInRiver(mapTmp, curPoint, deep, tmpList, 0, 1);

                if (!tmpList.Any())
                {
                    return null;
                }

                var pointIdx = rand.Next(tmpList.Count);
                mapTmp[tmpList[pointIdx].X, tmpList[pointIdx].Y] = deep;
                return tmpList[pointIdx];
            }

            return null;
        }

        private void AddPointToTmpListIfNotInRiver(int[,] mapTmp, Point curPoint, int deep, List<Point> tmpList, int dx, int dy)
        {
            if (mapTmp[curPoint.X + dx, curPoint.Y + dy] != deep)
            {
                var nextPosPoint = new Point(curPoint.X + dx, curPoint.Y + dy);
                tmpList.Add(nextPosPoint);
                if (mapTmp[curPoint.X - dx, curPoint.Y - dy] == deep)
                {
                    tmpList.Add(nextPosPoint);
                    tmpList.Add(nextPosPoint);
                    tmpList.Add(nextPosPoint);
                    tmpList.Add(nextPosPoint);
                }
            }
        }
    }

}
