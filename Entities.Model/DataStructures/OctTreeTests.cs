using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Entities.Model.DataStructures
{
    [TestFixture]
    public class OctTreeTests
    {
        [TestCase(0,0,0)]
        public void AddAPointAssertExists(int x, int y, int z)
        {
            var ut = new OctTree<object>(BoundingCuboid.Max, 10, new SimpleOctTreeDivisionStrategy<object>());
            var point3Int = new Point3Int(x, y, z);

            var point = new Point3Int<object>(point3Int, new object());
            ut.Add(point);

            Assert.Contains(point, ut.Points);
        }

        [TestCase()]
        public void DontAddPoint()
        {
            var ut = new OctTree<object>(BoundingCuboid.Max, 10,new SimpleOctTreeDivisionStrategy<object>());
            var point = new Point3Int<object>(Point3Int.Zero, new object());
            Assert.IsFalse(ut.Points.Contains(point));
        }

        [TestCase()]
        public void AddPointsExceedCapacityAssertEndUpInRightOct()
        {
            var obj = new object();
            var blf = new Point3Int<object>(-2, -2, -2, obj);
            var brf = new Point3Int<object>(2, -2, -2, obj);

            var blb = new Point3Int<object>(-2, -2, 2, obj);
            var brb = new Point3Int<object>(2, -2, 2, obj);

            var ulf = new Point3Int<object>(-2, 2, -2, obj);
            var urf = new Point3Int<object>(2, 2, -2, obj);

            var ulb = new Point3Int<object>(-2, 2, 2, obj);
            var urb = new Point3Int<object>(2, 2, 2, obj);

            var magicNumber9 = new Point3Int<object>(0, 0, 0, obj);

            var ut = new OctTree<object>(BoundingCuboid.Max, 8, new SimpleOctTreeDivisionStrategy<object>());

            ut.Add(blf);
            ut.Add(brf);
            ut.Add(blb);
            ut.Add(brb);
            ut.Add(ulf);
            ut.Add(urf);
            ut.Add(ulb);
            ut.Add(urb);

            Assert.IsNotNull(ut.Points);
            Assert.AreEqual(8, ut.Points.Length);
            Assert.IsNull(ut.ULF);
            Assert.IsNull(ut.URF);
            Assert.IsNull(ut.ULB);
            Assert.IsNull(ut.URB);
            Assert.IsNull(ut.BLF);
            Assert.IsNull(ut.BRF);
            Assert.IsNull(ut.BLB);
            Assert.IsNull(ut.BRB);

            ut.Add(magicNumber9);

            Assert.IsNull(ut.Points);
            Assert.IsNotNull(ut.ULF);
            Assert.IsNotNull(ut.URF);
            Assert.IsNotNull(ut.ULB);
            Assert.IsNotNull(ut.URB);
            Assert.IsNotNull(ut.BLF);
            Assert.IsNotNull(ut.BRF);
            Assert.IsNotNull(ut.BLB);
            Assert.IsNotNull(ut.BRB);

            Assert.IsTrue(ut.BLF.Points.Contains(blf));
            Assert.Contains(brf, ut.BRF.Points);
            Assert.Contains(blb, ut.BLB.Points);
            Assert.Contains(brb, ut.BRB.Points);

            Assert.Contains(urf, ut.URF.Points);
            Assert.Contains(ulf, ut.ULF.Points);
            Assert.Contains(ulb, ut.ULB.Points);
            Assert.Contains(urb, ut.URB.Points);

            Assert.Contains(magicNumber9, ut.BLF.Points);
        }


        [TestCase(-1,-1,-1,1,1,1,-1,-1,-1,1,1,1,0,0,0,true,TestName = "boundary = quad")]
        [TestCase(-1,-1,-1,1,1,1,1,1,1,2,2,2,0,0,0,false,TestName = "boundary outside quad")]
        [TestCase(-2,-2,-2,2,2,2,0,0,0,2,2,2,-1,-1,-1,false,TestName = "boundary inside quad, point outside boundary")]
        public void GetPointsInBoundingArea(int octMinX, int octMinY, int octMinZ, int octMaxX, int octMaxY, int octMaxZ,
            int boundaryMinX, int boundaryMinY, int boundaryMinZ, int boundaryMaxX, int boundaryMaxY, int boundaryMaxZ,
            int x, int y, int z,
            bool expected)
        {
            var octTreeBoundingBox = new BoundingCuboid(new Point3Int(octMinX, octMinY, octMinZ), new Point3Int(octMaxX, octMaxY, octMaxZ));

            var area = new BoundingCuboid(new Point3Int(boundaryMinX, boundaryMinY, boundaryMinZ), new Point3Int(boundaryMaxX, boundaryMaxY, boundaryMaxZ));

            var obj = new object();
            var point = new Point3Int<object>(x, y, z, obj);

            var ut = new OctTree<object>(octTreeBoundingBox, 10, new SimpleOctTreeDivisionStrategy<object>());
            ut.Add(point);

            var points = new List<Point3Int<object>>();
            ut.GetPointsWithinBoundary(area, ref points);

            Assert.AreEqual(expected, points.Contains(point));
        }
    }

 
}
