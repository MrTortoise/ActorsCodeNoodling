using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataStructures;
using NUnit.Framework;

namespace Entities.Model.DataStructures
{
    [TestFixture]
    class QuadTreeTests
    {
        [TestCase()]
        public void AddAPointAssertExists()
        {
            var ut = new QuadTree(Bounding2DBox.Max, 10, new SimpleQuadTreeDivisionStrategy());
            ut.Add(Point2Int.Zero);

            Assert.IsTrue(ut.Contains(Point2Int.Zero));
        }

        [TestCase()]
        public void DontAddAPointAssertNotExists()
        {
            var ut = new QuadTree(Bounding2DBox.Max, 10, new SimpleQuadTreeDivisionStrategy());
            Assert.IsFalse(ut.Contains(Point2Int.Zero));
        }

        [TestCase()]
        public void AddPointOutsideOfBoundary()
        {
            var ut = new QuadTree(new Bounding2DBox(new Point2Int(-1, -1), new Point2Int(1, 1)), 10, new SimpleQuadTreeDivisionStrategy());
            var point = new Point2Int(10,10);

            Assert.Throws<ArgumentOutOfRangeException>(() => ut.Add(point));
        }

        [TestCase()]
        public void AddMorePointsThanMaxAssertTheyEndUpInCorrectQuadrants()
        {
            var tl1 = new Point2Int(-10, 10);
            var tl2 = new Point2Int(-11, 10);
            var tl3 = new Point2Int(-12, 10);

            var tr1 = new Point2Int(10, 10);
            var tr2 = new Point2Int(10, 11);
            var tr3 = new Point2Int(10, 12);

            var bl1 = new Point2Int(-10, -10);
            var bl2 = new Point2Int(-10, -11);
            var bl3 = new Point2Int(-10, -12);

            var br1 = new Point2Int(10, -10);
            var br2 = new Point2Int(10, -11);
            var br3 = new Point2Int(10, -12);

            var number10 = new Point2Int(-10, 12);

            var ut = new QuadTree(Bounding2DBox.Max, 12, new SimpleQuadTreeDivisionStrategy());
            ut.Add(tl1);
            ut.Add(tl2);
            ut.Add(tl3);
            ut.Add(tr1);
            ut.Add(tr2);
            ut.Add(tr3);
            ut.Add(bl1);
            ut.Add(bl2);
            ut.Add(bl3);
            ut.Add(br1);
            ut.Add(br2);
            ut.Add(br3);

            Assert.IsNotNull(ut.Points);
            Assert.AreEqual(12, ut.Points.Length);
            Assert.IsNull(ut.NorthEast);
            Assert.IsNull(ut.NorthWest);
            Assert.IsNull(ut.SouthEast);
            Assert.IsNull(ut.SouthWest);

            ut.Add(number10);

            Assert.IsNull(ut.Points);
            Assert.IsNotNull(ut.NorthEast);
            Assert.IsNotNull(ut.NorthWest);
            Assert.IsNotNull(ut.SouthEast);
            Assert.IsNotNull(ut.SouthWest);

            Assert.IsTrue(ut.NorthEast.Points.Contains(tr1));
            Assert.IsTrue(ut.NorthEast.Points.Contains(tr2));
            Assert.IsTrue(ut.NorthEast.Points.Contains(tr3));

            Assert.IsTrue(ut.NorthWest.Contains(tl1));
            Assert.IsTrue(ut.NorthWest.Contains(tl2));
            Assert.IsTrue(ut.NorthWest.Contains(tl3));
            Assert.IsTrue(ut.NorthWest.Contains(number10));

            Assert.IsTrue(ut.SouthWest.Points.Contains(bl1));
            Assert.IsTrue(ut.SouthWest.Points.Contains(bl2));
            Assert.IsTrue(ut.SouthWest.Points.Contains(bl3));

            Assert.IsTrue(ut.SouthEast.Points.Contains(br1));
            Assert.IsTrue(ut.SouthEast.Points.Contains(br2));
            Assert.IsTrue(ut.SouthEast.Points.Contains(br3));
        }

        [TestCase(-1,-1,1,1,-1,-1,1,1,0,0,true,TestName = "boundary = quad")]
        [TestCase(-1, -1, 1, 1, 1, 1, 2, 2, 0, 0, false, TestName = "boundary outside quad")]
        [TestCase(-2, -2, 2, 2, 0, 0, 2, 2, -1, -1, false, TestName = "boundary inside quad, point outside boundary")]
        [TestCase(-2, -2, 2, 2, 0, 0, 2, 2, 1, 1, true, TestName = "boundary inside quad, point inside boundary")]
        public void PointInAreaAssert(
            int quadMinx, int quadMiny, int quadMaxx, int quadMaxy,
            int boundaryMinx, int boundaryMiny, int boundaryMaxx, int boundaryMaxy, 
            int pointx, int pointy, 
            bool expected)
        {
            var quadBoundingBox = new Bounding2DBox(new Point2Int(quadMinx, quadMiny), new Point2Int(quadMaxx, quadMaxy));
            var area = new Bounding2DBox(new Point2Int(boundaryMinx, boundaryMiny), new Point2Int(boundaryMaxx, boundaryMaxy));

            var point = new Point2Int(pointx, pointy);

            var ut = new QuadTree(quadBoundingBox, 10, new SimpleQuadTreeDivisionStrategy());
            ut.Add(point);

            var points = new List<Point2Int>();
            ut.GetPointsInArea(area, ref points);

            Assert.AreEqual(expected, points.Contains(point));
        }
    }
}
