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
            var ut = new OctTree(BoundingCuboid.Max, 10, new SimpleOctTreeDivisionStrategy());
            var point3Int = new Point3Int(x, y, z);
            ut.Add(point3Int);

            Assert.Contains(point3Int,ut.Points);
        }

        [TestCase()]
        public void DontAddPoint()
        {
            var ut = new OctTree(BoundingCuboid.Max, 10,new SimpleOctTreeDivisionStrategy());
            Assert.IsFalse(ut.Points.Contains(Point3Int.Zero));
        }

        [TestCase()]
        public void AddPointsExceedCapacityAssertEndUpInRightOct()
        {
            var blf = new Point3Int(-2, -2, -2);
            var brf = new Point3Int(2, -2, -2);

            var blb = new Point3Int(-2, -2, 2);
            var brb = new Point3Int(2, -2, 2);

            var ulf = new Point3Int(-2, 2, -2);
            var urf = new Point3Int(2, 2, -2);

            var ulb = new Point3Int(-2, 2, 2);
            var urb = new Point3Int(2, 2, 2);

            var magicNumber9 = new Point3Int(0, 0, 0);

            var ut = new OctTree(BoundingCuboid.Max, 8, new SimpleOctTreeDivisionStrategy());

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

            var point = new Point3Int(x, y, z);

            var ut = new OctTree(octTreeBoundingBox, 10, new SimpleOctTreeDivisionStrategy());
            ut.Add(point);

            var points = new List<Point3Int>();
            ut.GetPointsWithinBoundary(area, ref points);

            Assert.AreEqual(expected, points.Contains(point));
        }
    }

 
}
