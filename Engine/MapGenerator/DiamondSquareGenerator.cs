using System;
using System.Collections.Generic;

namespace Engine.MapGenerator
{
    public class DiamondSquareGenerator
    {
        public virtual float[,] GenerateBaseMap(int power = 7, float maxheight = 256, int seedCount = 8)
        {
            var size = (int)Math.Pow(2, power) + 1;

            var resultMap = new float[size, size];

            GenerateSeeds(maxheight, seedCount, size, resultMap);

            GenerateSeededMap(power, maxheight, resultMap);

            return resultMap;
        }

        protected virtual void GenerateSeeds(float maxheight, int seedCount, int size, float[,] resultMap)
        {
            var seedList = GenerateSeedPoints(seedCount, size);

            foreach (var point in seedList)
            {
                resultMap[point.X, point.Y] = 0f; //(float) random.NextDouble(); //*maxheight;
            }
        }

        protected virtual List<Point> GenerateSeedPoints(int seedCount, int size)
        {
            var seedList = new List<Point>();

            seedList.AddRange(new[]
            {new Point(0, 0), new Point(0, size - 1), new Point(size - 1, 0), new Point(size - 1, size - 1)});
            
            for (int i = 0; i < seedCount; i++)
            {
                //seedList.Add(new Point(random.Next(size), random.Next(size)));
            }
            
            return seedList;
        }

        public virtual void GenerateSeededMap(int power, float maxheight, float[,] resultMap)
        {
            var size = 1 << power;

            for (int i = 0; i < power; i++)
            {
                var distance = size >> i;
                var height = maxheight * distance / (size * 256);
                for (int x = 0; x < 1 << i; x++)
                {
                    for (int y = 0; y < 1 << i; y++)
                    {
                        SquareStep(resultMap, distance, x, y, height);
                    }
                }
                
                distance = distance/2;
                height = maxheight * distance / (size * 256);
                for (int x = -1; x < (int)Math.Pow(2, i + 1); x++)
                {
                    for (int y = Math.Abs(x)%2*distance; y <= size; y += 2*distance)
                    {
                        DiamondStep(resultMap, distance, x, y, height, size);
                    }
                }
            }
        }

        protected virtual void DiamondStep(float[,] map, int distance, int x, int y, float height, int size)
        {

            if (map[x * distance + distance, y] != 0)
                return;

            map[x*distance + distance, y] =
                (
                    map[(size + x*distance)%size, y]
                    + map[(size + x*distance + distance)%size + distance, y]
                    + map[x*distance%size + distance, (size + y - distance)%size]
                    + map[x*distance%size + distance, (size + y)%size + distance]
                    )/4 + GetDisplace(height);

          //  map[x * distance + distance, y] = Normalize(map[x * distance + distance, y]);
        }

        protected virtual void SquareStep(float[,] map, int distance, int x, int y, float height)
        {
            if (map[x * distance + distance / 2, y * distance + distance / 2] != 0)
                return;

            map[x*distance + distance/2, y*distance + distance/2] =
                (
                    map[x*distance, y*distance]
                    + map[x*distance, y*distance + distance]
                    + map[x*distance + distance, y*distance]
                    + map[x*distance + distance, y*distance + distance]
                    )/4 + GetDisplace(height);

           // map[x*distance + distance/2, y*distance + distance/2] =
           //     Normalize(map[x*distance + distance/2, y*distance + distance/2]);
        }
        Random random = new Random();

        private float GetDisplace(float height)
        {
            return (2*(float) random.NextDouble() - 1.0f)*height;
        }

        private float Normalize(float p)
        {
            return p > 1.0f ? 1.0f : p < -1.0f ? -1.0f : p;
        }
    }
}
