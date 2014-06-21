using System;
using System.Collections.Generic;
using System.Linq;
using Game.Engine;
using Game.Engine.MapGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class MapGeneratorTests
    {
        [TestMethod]
        public void CheckSeededPointsShouldNotChangeAfterGeneration()
        {
            var testDiamondSquareGenerator = new TestDiamondSquareGenerator();
            var resultMap = testDiamondSquareGenerator.GenerateBaseMap(2, 16, 1);

            testDiamondSquareGenerator._seedPoints.ForEach(p => Assert.AreEqual(testDiamondSquareGenerator._seeds[p], resultMap[p.X, p.Y]));
        }

        [TestMethod]
        public void CheckOrderOfGenerationSteps()
        {
            var testDiamondSquareGenerator = new TestDiamondSquareGenerator();

            var expectedMap = new float[5, 5];
            expectedMap[0, 0] = 64;
            expectedMap[1, 0] = 1021;
            expectedMap[2, 0] = 760;
            expectedMap[3, 0] = 799;
            expectedMap[4, 0] = 256;

            expectedMap[0, 1] = 901;
            expectedMap[1, 1] = 976;
            expectedMap[2, 1] = 985;
            expectedMap[3, 1] = 844;
            expectedMap[4, 1] = 769;

            expectedMap[0, 2] = 1720;
            expectedMap[1, 2] = 1585;
            expectedMap[2, 2] = 1360;
            expectedMap[3, 2] = 1135;
            expectedMap[4, 2] = 1000;

            expectedMap[0, 3] = 2359;
            expectedMap[1, 3] = 2284;
            expectedMap[2, 3] = 1735;
            expectedMap[3, 3] = 1336;
            expectedMap[4, 3] = 1411;

            expectedMap[0, 4] = 4096;
            expectedMap[1, 4] = 2329;
            expectedMap[2, 4] = 1960;
            expectedMap[3, 4] = 1291;
            expectedMap[4, 4] = 1024;

            var actualMap = new float[5,5];
            actualMap[0, 0] = 64;
            actualMap[4, 0] = 256;
            actualMap[0, 4] = 4096;
            actualMap[4, 4] = 1024;

            testDiamondSquareGenerator.GenerateSeededMap(2, 0, actualMap);

            CollectionAssert.AreEqual(expectedMap, actualMap);
        }
    }

    class TestDiamondSquareGenerator:DiamondSquareGenerator
    {
        internal List<Point> _seedPoints = new List<Point>();
        internal Dictionary<Point, float> _seeds = new Dictionary<Point, float>();

        protected override List<Point> GenerateSeedPoints(int seedCount, int size)
        {
            var seedPoints = base.GenerateSeedPoints(seedCount, size);

            _seedPoints.Clear();
            _seedPoints.AddRange(seedPoints);

            return seedPoints;
        }

        protected override void GenerateSeeds(float maxheight, int seedCount, int size, float[,] resultMap)
        {
            base.GenerateSeeds(maxheight, seedCount, size, resultMap);

            _seeds = _seedPoints.Distinct().ToDictionary(p => p, p => resultMap[p.X, p.Y]);
        }
    }
}
