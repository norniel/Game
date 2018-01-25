using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.MapGenerator
{
    public class VoronoyGenerator
    {
        private readonly Random _random = new Random();
        public float[,] GenerateMapWithVoronoyDiagrams(int power, int seedCounts)
        {
            var size = 1 << power + 1;

            seedCounts = seedCounts < 2 ? 2 : seedCounts;

            var resultMap = new float[size, size];

            var seedPoints = new List<Point>();

            for (int i = 0; i < seedCounts; i++)
            {
                seedPoints.Add(new Point(_random.Next(size), _random.Next(size)));
            }

            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                    var distances = seedPoints.Select(p => GetDistance(p.X, p.Y, i, j)).OrderBy(d => d).ToList();
                    resultMap[i,j] = distances[1] - distances[0];
                }
            }

            resultMap = NormalizeMap(resultMap);

            return resultMap;
        }

        private float[,] NormalizeMap(float[,] mapToNormalize)
        {
            float maxDistance = 0;
            for (int i = 0; i < mapToNormalize.GetLength(0); i++)
            {
                for (int j = 0; j < mapToNormalize.GetLength(1); j++)
                {
                    maxDistance = maxDistance < mapToNormalize[i, j] ? mapToNormalize[i, j] : maxDistance;
                }
            }

            if (maxDistance <= 0)
                return mapToNormalize;

            var resultMap = new float[mapToNormalize.GetLength(0), mapToNormalize.GetLength(1)];

            for (int i = 0; i < mapToNormalize.GetLength(0); i++)
            {
                for (int j = 0; j < mapToNormalize.GetLength(1); j++)
                {
                    resultMap[i,j] = mapToNormalize[i, j]/ maxDistance;
                }
            }

            return resultMap;
        }

        private float GetDistance(int px, int py, int x, int y)
        {
            return (px - x)*(px - x) + (py - y)*(py - y);
        }
    }
}
