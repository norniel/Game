using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Interfaces;
using Engine.Tools;
using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Interfaces;
using Engine.Tools;


namespace Tests
{
    [TestClass]
    public class ShdowCastingTests
    {
        [TestMethod]
        public void TestOneCloseObject1()
        {
            var expectedMap = @"*.......*"+
                               "**.....**"+
                               "***...***"+
                               "*********"+
                               "****.****";
            
            var map = CreateMap(9, 5, false, new Point(4, 3));
            var visibleCells = ShadowCasting.For(new Point(4, 4), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(9, 5, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCloseObject2()
        {
            var expectedMap = @"****.****" +
                               "*********" +
                               "***...***" +
                               "**.....**" +
                               "*.......*";

            var map = CreateMap(9, 5, false, new Point(4, 1));
            var visibleCells = ShadowCasting.For(new Point(4, 0), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(9, 5, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCloseObject3()
        {
            var expectedMap = @"*****" +
                               ".****" +
                               "..***" +
                               "...**" +
                               "...*." +
                               "...**" +
                               "..***" +
                               ".****" +
                               "*****";

            var map = CreateMap(5, 9, false, new Point(3, 4));
            var visibleCells = ShadowCasting.For(new Point(4, 4), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(5, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCloseObject4()
        {
            var expectedMap = @"*****" +
                               "****." +
                               "***.." +
                               "**..." +
                               ".*..." +
                               "**..." +
                               "***.." +
                               "****." +
                               "*****";

            var map = CreateMap(5, 9, false, new Point(1, 4));
            var visibleCells = ShadowCasting.For(new Point(0, 4), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(5, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDistantObject1()
        {
            var expectedMap = @"**************" +
                               "*.************" +
                               "**************" +
                               "******..******" +
                               "********......" +
                               "**********...." + 
                               "***********..." +
                               "*************." +
                               "**************";

            var map = CreateMap(14, 9, false, new Point(4, 2));
            var visibleCells = ShadowCasting.For(new Point(1, 1), 15, map).GetVisibleCells().Distinct().ToList();

            AssertMap(14, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDistantObject2()
        {
            var expectedMap = @"**************" +
                               "************.*" +
                               "**************" +
                               "******..******" +
                               "......********" +
                               "....**********" +
                               "...***********" +
                               ".*************" +
                               "**************";

            var map = CreateMap(14, 9, false, new Point(9, 2));
            var visibleCells = ShadowCasting.For(new Point(12, 1), 15, map).GetVisibleCells().Distinct().ToList();

            AssertMap(14, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDistantObject3()
        {
            var expectedMap = @"**************" +
                               "*************." +
                               "***********..." +
                               "**********...." +
                               "********......" +
                               "******..******" +
                               "**************" +
                               "*.************" +
                               "**************";

            var map = CreateMap(14, 9, false, new Point(4, 6));
            var visibleCells = ShadowCasting.For(new Point(1, 7), 15, map).GetVisibleCells().Distinct().ToList();

            AssertMap(14, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDistantObject4()
        {
            var expectedMap = @"**************" +
                               ".*************" +
                               "...***********" +
                               "....**********" +
                               "......********" +
                               "******..******" +
                               "**************" +
                               "************.*" +
                               "**************";

            var map = CreateMap(14, 9, false, new Point(9, 6));
            var visibleCells = ShadowCasting.For(new Point(12, 7), 15, map).GetVisibleCells().Distinct().ToList();

            AssertMap(14, 9, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCornerPicker1()
        {
            var expectedMap = @".*********." +
                               "..*******.." +
                               "...*****..." +
                               "....***...." +
                               "....*.*...." +
                               "....***...." +
                               "...*****..." +
                               "...........";

            var map = CreateMap(11, 8, false, new Point(4, 4), new Point(6, 4), new Point(0, 6), new Point(1, 6), new Point(2, 6), new Point(3, 6),
                new Point(4, 6), new Point(5, 6), new Point(6, 6), new Point(7, 6), new Point(8, 6), new Point(9, 6), new Point(10, 6));
            var visibleCells = ShadowCasting.For(new Point(5, 4), 6, map).GetVisibleCells().Distinct().ToList();

            AssertMap(11, 8, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCornerPicker2()
        {
            var expectedMap = @"..........." +
                               "...*****..." +
                               "....***...." +
                               "....*.*...." +
                               "....***...." +
                               "...*****..." +
                               "..*******.." +
                               ".*********.";

            var map = CreateMap(11, 8, false, new Point(4, 3), new Point(6, 3), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1),
                new Point(4, 1), new Point(5, 1), new Point(6, 1), new Point(7, 1), new Point(8, 1), new Point(9, 1), new Point(10, 1));
            var visibleCells = ShadowCasting.For(new Point(5, 3), 6, map).GetVisibleCells().Distinct().ToList();

            AssertMap(11, 8, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneCornerPicker3()
        {
            var expectedMap = @"........" +
                               "*......." +
                               "**......" +
                               "***...*." +
                               "*******." +
                               "****.**." +
                               "*******." +
                               "***...*." +
                               "**......" +
                               "*......." +
                               "........";

            var map = CreateMap(8, 11, false, new Point(4, 4), new Point(4, 6), new Point(6, 0), new Point(6, 1), new Point(6, 2), new Point(6, 3),
                new Point(6, 4), new Point(6, 5), new Point(6, 6), new Point(6, 7), new Point(6, 8), new Point(6, 9), new Point(6, 10));
            var visibleCells = ShadowCasting.For(new Point(4, 5), 6, map).GetVisibleCells().Distinct().ToList();

            AssertMap(8, 11, expectedMap, visibleCells);
        }
        [TestMethod]
        public void TestOneCornerPicker4()
        {
            var expectedMap = @"........" +
                               ".......*" +
                               "......**" +
                               ".*...***" +
                               ".*******" +
                               ".**.****" +
                               ".*******" +
                               ".*...***" +
                               "......**" +
                               ".......*" +
                               "........";

            var map = CreateMap(8, 11, false, new Point(3, 4), new Point(3, 6), new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3),
                new Point(1, 4), new Point(1, 5), new Point(1, 6), new Point(1, 7), new Point(1, 8), new Point(1, 9), new Point(1, 10));
            var visibleCells = ShadowCasting.For(new Point(3, 5), 6, map).GetVisibleCells().Distinct().ToList();

            AssertMap(8, 11, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDiagonalObject1()
        {
            var expectedMap = @"..**" +
                               "..**" +
                               "****" +
                               "***." ;

            var map = CreateMap(4, 4, false, new Point(2, 2));
            var visibleCells = ShadowCasting.For(new Point(3, 3), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(4, 4, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDiagonalObject2()
        {
            var expectedMap = @"**.." +
                               "**.." +
                               "****" +
                               ".***";

            var map = CreateMap(4, 4, false, new Point(1, 2));
            var visibleCells = ShadowCasting.For(new Point(0, 3), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(4, 4, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDiagonalObject3()
        {
            var expectedMap = @".***" +
                               "****" +
                               "**.." +
                               "**..";

            var map = CreateMap(4, 4, false, new Point(1, 1));
            var visibleCells = ShadowCasting.For(new Point(0, 0), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(4, 4, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestOneDiagonalObject4()
        {
            var expectedMap = @"***." +
                               "****" +
                               "..**" +
                               "..**";

            var map = CreateMap(4, 4, false, new Point(2, 1));
            var visibleCells = ShadowCasting.For(new Point(3, 0), 4, map).GetVisibleCells().Distinct().ToList();

            AssertMap(4, 4, expectedMap, visibleCells);
        }


        [TestMethod]
        public void TestTwoDiagonalObjects1()
        {
            var expectedMap = @"*.****" +
                               ".*.***" + 
                               "*.****" + 
                               "******" +
                               "******" +
                               "*****.";

            var map = CreateMap(6, 6, false, new Point(2, 3), new Point(3, 2));
            var visibleCells = ShadowCasting.For(new Point(5, 5), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestTwoDiagonalObjects2()
        {
            var expectedMap = @"****.*" +
                               "***.*." +
                               "****.*" +
                               "******" +
                               "******" +
                               ".*****";

            var map = CreateMap(6, 6, false, new Point(2, 2), new Point(3, 3));
            var visibleCells = ShadowCasting.For(new Point(0, 5), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestTwoDiagonalObjects3()
        {
            var expectedMap = @"*****." +
                               "******" +
                               "******" +
                               "*.****" +
                               ".*.***" +
                               "*.****";

            var map = CreateMap(6, 6, false, new Point(2, 2), new Point(3, 3));
            var visibleCells = ShadowCasting.For(new Point(5, 0), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestTwoDiagonalObjects4()
        {
            var expectedMap = @".*****" +
                               "******" +
                               "******" +
                               "****.*" +
                               "***.*." +
                               "****.*";

            var map = CreateMap(6, 6, false, new Point(2, 3), new Point(3, 2));
            var visibleCells = ShadowCasting.For(new Point(0, 0), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestCornerObjects1()
        {
            var expectedMap = @"..****" +
                               "...***" +
                               "*..***" +
                               "******" +
                               "******" +
                               "*****.";

            var map = CreateMap(6, 6, false, new Point(2, 3), new Point(3, 2), new Point(3, 3));
            var visibleCells = ShadowCasting.For(new Point(5, 5), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestCornerObjects2()
        {
            var expectedMap = @"****.." +
                               "***..." +
                               "***..*" +
                               "******" +
                               "******" +
                               ".*****";

            var map = CreateMap(6, 6, false, new Point(2, 2), new Point(3, 3), new Point(2, 3));
            var visibleCells = ShadowCasting.For(new Point(0, 5), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestCornerObjects3()
        {
            var expectedMap = @"*****." +
                               "******" +
                               "******" +
                               "*..***" +
                               "...***" +
                               "..****";

            var map = CreateMap(6, 6, false, new Point(2, 2), new Point(3, 3), new Point(3, 2));
            var visibleCells = ShadowCasting.For(new Point(5, 0), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        [TestMethod]
        public void TestCornerObjects4()
        {
            var expectedMap = @".*****" +
                               "******" +
                               "******" +
                               "***..*" +
                               "***..." +
                               "****..";

            var map = CreateMap(6, 6, false, new Point(2, 3), new Point(3, 2), new Point(2, 2));
            var visibleCells = ShadowCasting.For(new Point(0, 0), 7, map).GetVisibleCells().Distinct().ToList();

            AssertMap(6, 6, expectedMap, visibleCells);
        }

        private IMap CreateMap(int sizeX, int sizeY, bool addObjects, params Point[] objectPoints)
        {
            var map = new TestMap(sizeX, sizeY, addObjects);
            map.AddNotPassable(objectPoints);
            return map;
        }

        private void AssertMap(int sizeX, int sizeY, string expectedMap, List<PointWithDistance> actualVisibleCells)
        {
            var expectedVisibleCells = expectedMap.Select((x, i) => new {C = x, I = i})
                .Where(x => x.C == '*')
                .Select(x => new PointWithDistance() {Point = new Point(x.I%sizeX, x.I/sizeX)})
                .ToList();

            CollectionAssert.AreEquivalent(expectedVisibleCells, actualVisibleCells, GetActualMap(sizeX, sizeY, actualVisibleCells));
        }

        private string GetActualMap(int sizeX, int sizeY, List<PointWithDistance> actualVisibleCells)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine();

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (actualVisibleCells.Any(p => p.Point.X == j && p.Point.Y == i))
                    {
                        stringBuilder.Append("*");
                    }
                    else
                    {
                        stringBuilder.Append(".");
                    }
                }
                stringBuilder.AppendLine();
            }
            
            return stringBuilder.ToString();
        }

        private class TestMap : IMap
        {
            private readonly  FixedObject[,] _localMap;
            public TestMap(int sizeX, int sizeY, bool addObjects)
            {
                _localMap = new FixedObject[sizeX, sizeY];

                if (addObjects)
                {
                    for (int i = 0; i < _localMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < _localMap.GetLength(1); j++)
                        {
                            _localMap[i, j] = new FixedObject();
                        }
                    }
                }
            }

            public void AddNotPassable(params Point[] objectPoints)
            {
                for (int i = 0; i < objectPoints.GetLength(0); i++)
                {
                    _localMap[objectPoints[i].X, objectPoints[i].Y] = new NotPassableObject();
                }
            }

            public Rect GetSize()
            {
                return new Rect(0, 0, (uint)_localMap.GetLength(0), (uint)_localMap.GetLength(1));
            }

            public FixedObject GetObjectFromCell(Point cell)
            {
                return _localMap[cell.X, cell.Y];
            }
        }

        private class NotPassableObject : FixedObject
        {
            public NotPassableObject()
            {
                IsPassable = false;
            }
        }
    }

}
