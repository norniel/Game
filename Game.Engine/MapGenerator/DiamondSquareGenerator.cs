using System;
using System.Collections.Generic;

namespace Game.Engine.MapGenerator
{
    public class DiamondSquareGenerator
    {
        public virtual int[,] GenerateBaseMap(int power = 7, int maxheight = 256, int seedCount = 8)
        {
            var size = (int)Math.Pow(2, power) + 1;

            var resultMap = new int[size, size];

            GenerateSeeds(maxheight, seedCount, size, resultMap);

            GenerateSeededMap(power, maxheight, resultMap);

            return resultMap;
        }

        protected virtual void GenerateSeeds(int maxheight, int seedCount, int size, int[,] resultMap)
        {
            var random = new Random();
            var seedList = GenerateSeedPoints(seedCount, size);

            foreach (var point in seedList)
            {
                resultMap[point.X, point.Y] = random.Next(maxheight);
            }
        }

        protected virtual List<Point> GenerateSeedPoints(int seedCount, int size)
        {
            var random = new Random();
            var seedList = new List<Point>();

            seedList.AddRange(new[]
            {new Point(0, 0), new Point(0, size - 1), new Point(size - 1, 0), new Point(size - 1, size - 1)});

            for (int i = 0; i < seedCount; i++)
            {
                seedList.Add(new Point(random.Next(size), random.Next(size)));
            }

            return seedList;
        }

        public virtual void GenerateSeededMap(int power, int maxheight, int[,] resultMap)
        {
            var size = (int)Math.Pow(2, power) + 1;

            for (int i = 0; i < power; i++)
            {
                var distance = (size - 1) >> i;
                var height = maxheight >> (i + 1);
                for (int x = 0; x <= i; x++)
                {
                    for (int y = 0; y <= i; y++)
                    {
                        SquareStep(resultMap, distance, x, y, height);
                    }
                }

                distance = distance/2;
                height = height >> 1;
                for (int x = -1; x < (int)Math.Pow(2, i + 1); x++)
                {
                    for (int y = (Math.Abs(x)%2)*distance; y < size; y += 2*distance)
                    {
                        DiamondStep(resultMap, distance, x, y, height, size - 1);
                    }
                }
            }
        }

        protected virtual void DiamondStep(int[,] map, int distance, int x, int y, int height, int size)
        {
            var random = new Random();

            if (map[x * distance + distance, y] != 0)
                return;

            map[x * distance + distance, y] =
                (
                    map[(size + x * distance) % size, y]
                    + map[(size + x * distance + distance) % size + distance, y]
                    + map[(x * distance)%size + distance, (size + y - distance) % size]
                    + map[(x * distance) % size + distance, (size + y) % size + distance]
                ) / 4 + random.Next(2 * height) - height;
        }

        protected virtual void SquareStep(int[,] map, int distance, int x, int y, int height)
        {
            var random = new Random();

            if (map[x * distance + distance / 2, y * distance + distance / 2] != 0)
                return;

            map[x * distance + distance / 2, y * distance + distance / 2] = 
                (
                    map[x * distance, y * distance] 
                    + map[x * distance, y * distance + distance] 
                    + map[x * distance + distance, y * distance]
                    + map[x * distance + distance, y * distance + distance]
                ) / 4 + random.Next(2*height) - height;
        }
    }
}
