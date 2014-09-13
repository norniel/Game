using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Game.Engine.MapGenerator
{
    public class HydraliticErosion2
    {
        private float KWaterDrop = 0.01f;
        private float KSolubility = 0.01f;
        private float KEvaporation = 0;//0.1f;
        private float KSedimentCapacity = 0.01f;

        readonly float[,] _resultMap;
        readonly float[,] _waterMap;
      //  readonly float[,] _sedimentMap;
        private readonly int _itterationperStep;
        private readonly float _maxHeight;
        private readonly float _lowerMaxHeight;

        public HydraliticErosion2(float[,] mapToApply, int itterationperStep)
        {
            _resultMap = new float[mapToApply.GetLength(0), mapToApply.GetLength(1)];
            _waterMap = new float[mapToApply.GetLength(0), mapToApply.GetLength(1)];
            //_sedimentMap = new float[mapToApply.GetLength(0), mapToApply.GetLength(1)];
            float minHeight = 0;

            for (int i = 0; i < mapToApply.GetLength(0); i++)
            {
                for (int j = 0; j < mapToApply.GetLength(1); j++)
                {
                    _resultMap[i, j] = mapToApply[i, j];
                    _maxHeight = Math.Max(_maxHeight, _resultMap[i, j]);
                    minHeight = Math.Min(minHeight, _resultMap[i, j]);
                }
            }

            _lowerMaxHeight = _maxHeight - 0.1f * (_maxHeight - minHeight);

            _itterationperStep = itterationperStep;
        }

        public float[,] ApplyErosion()
        {
            for (int i = 0; i < _itterationperStep; i++)
            {
                MakeErosionItteration(_waterMap, _resultMap, null/*, ((i/10) % 2) == 0*/);
            }

            return _resultMap;
        }

        public float[,] GetWaterMap()
        {
            return _waterMap;
        }

        private void MakeErosionItteration(float[,] waterMap, float[,] resultMap, float[,] sedimentMap, bool rain = true)
        {
            if (rain)
                Step1_MakeRain(waterMap);
         //   Step2_GetSediments(resultMap, waterMap, sedimentMap);
            Step3_Distribution(resultMap, waterMap, sedimentMap);
            Step4_Evaporation(resultMap, waterMap, sedimentMap);
        }

        private void Step4_Evaporation(float[,] resultMap, float[,] waterMap, float[,] sedimentMap)
        {
            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                    waterMap[i, j] -= waterMap[i, j] * KEvaporation;

                  //  var maxSedimentInWater = waterMap[i, j] * KSedimentCapacity;
                 //   var deltaSediment = Math.Max(0, sedimentMap[i, j] - maxSedimentInWater);
                    //sedimentMap[i, j] -= deltaSediment;
                    //resultMap[i, j] += deltaSediment;

                   // resultMap[i, j] = GetNewValue(resultMap[i, j]);
                    waterMap[i, j] = GetNewValue(waterMap[i, j]);
                   // sedimentMap[i, j] = GetNewValue(sedimentMap[i, j]);
                }
            }
        }

        private void Step3_Distribution(float[,] resultMap, float[,] waterMap, float[,] sedimentMap)
        {
            var deltaWaterMap = new float[waterMap.GetLength(0), waterMap.GetLength(1)];
            //var deltaSedimentMap = new float[sedimentMap.GetLength(0), sedimentMap.GetLength(1)];

            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                    var totalH = resultMap[i, j] + waterMap[i, j];
                    var lowerNeighbours = GetLowNeighbours(i, j, resultMap, waterMap);
                    if (lowerNeighbours.Count <= 0 || waterMap[i, j] <= 0)
                        continue;

                    var sumTotalH = lowerNeighbours.Sum(p => GetTotalH(p.X, p.Y, resultMap, waterMap));

                    if (sumTotalH <= 0)
                        continue;

                    var avgTotalH = sumTotalH / lowerNeighbours.Count; 
                    var minWater = Math.Min(totalH - avgTotalH, waterMap[i, j]);

                    deltaWaterMap[i, j] -= minWater;
                  //  deltaSedimentMap[i, j] -= (minWater * sedimentMap[i, j]) / waterMap[i, j];

                    lowerNeighbours.ForEach(
                        p =>
                        {
                            var currentDeltaWater =
                                minWater * (totalH - GetTotalH(p.X, p.Y, resultMap, waterMap)) / (totalH * lowerNeighbours.Count - sumTotalH);
                            deltaWaterMap[p.X, p.Y] += currentDeltaWater;

                          //  deltaSedimentMap[p.X, p.Y] += (currentDeltaWater * sedimentMap[i, j]) / waterMap[i, j];
                        });
                }
            }

            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                    waterMap[i, j] += deltaWaterMap[i, j];
                 //   sedimentMap[i, j] += deltaSedimentMap[i, j];
                    waterMap[i, j] = GetNewValue(waterMap[i, j]);
                  //  sedimentMap[i, j] = GetNewValue(sedimentMap[i, j]);
                }
            }
        }

        private List<Point> GetLowNeighbours(int x, int y, float[,] resultMap, float[,] waterMap)
        {
            var totalH = GetTotalH(x, y, resultMap, waterMap);
            return GetNeighbours(x, y, resultMap.GetLength(0)).Where(p => GetTotalH(p.X, p.Y, resultMap, waterMap) < totalH).ToList();
        }

        private float GetTotalH(int x, int y, float[,] resultMap, float[,] waterMap)
        {
            return resultMap[x, y] + waterMap[x, y];
        }

        private IEnumerable<Point> GetNeighbours(int x, int y, int size)
        {
            if (x > 0)
            {
                yield return new Point(x - 1, y);
/*
                if (y > 0)
                {
                    yield return new Point(x - 1, y - 1);
                }

                if (y < size - 1)
                {
                    yield return new Point(x - 1, y + 1);
                }*/
            }

            if (x < size - 1)
            {
                yield return new Point(x + 1, y);
                /*
                if (y > 0)
                {
                    yield return new Point(x + 1, y - 1);
                }

                if (y < size - 1)
                {
                    yield return new Point(x + 1, y + 1);
                }*/
            }

            if (y > 0)
            {
                yield return new Point(x, y - 1);
            }

            if (y < size - 1)
            {
                yield return new Point(x, y + 1);
            }
        }

        private void Step2_GetSediments(float[,] resultMap, float[,] waterMap, float[,] sedimentMap)
        {
            for (int i = 0; i < resultMap.GetLength(0); i++)
            {
                for (int j = 0; j < resultMap.GetLength(1); j++)
                {
                 //   resultMap[i, j] = GetNewValue(resultMap[i, j] - KSolubility * waterMap[i, j]);
                  //  sedimentMap[i, j] = GetNewValue(sedimentMap[i, j] + KSolubility * waterMap[i, j]);
                }
            }
        }

        private void Step1_MakeRain(float[,] waterMap)
        {
            for (int i = 0; i < waterMap.GetLength(0); i++)
            {
                for (int j = 0; j < waterMap.GetLength(1); j++)
                {
                    if (_resultMap[i, j] == _maxHeight  && _resultMap[i, j] > _lowerMaxHeight)
                    {
                        waterMap[i, j] += 0.01f; //KWaterDrop;
                    }
                    else
                        waterMap[i, j] += 0; //KWaterDrop;
                }
            }
        }

        private float GetNewValue(float prevValue)
        {
            return Double.IsNaN(prevValue) ? 0 : prevValue;// < 0.000001f ? 0 : prevValue;
        }
    }
}
