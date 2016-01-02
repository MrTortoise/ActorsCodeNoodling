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
    public class BoundingCuboidTests
    {
        [TestCase(1,2,3,4,5,6)]
        public void AssertValuesHold(int llx, int lly, int llz, int urx, int ury, int urz)
        {
            var lowerLeft = new Point3Int(llx, lly, llz);
            var upperRight = new Point3Int(urx, ury, urz);

            var ut = new BoundingCuboid(lowerLeft,upperRight);

            Assert.AreEqual(lowerLeft,ut.LowerLeft);
            Assert.AreEqual(upperRight, ut.UpperRight);
        }

        [TestCase(1,1,1,-1,-1,-1,true)]
        [TestCase(0,0,0,0,0,0, false)]
        public void AssertLowerLeftIsLessThanUpperRight(int llx, int lly, int llz, int urx, int ury, int urz, bool expectThrow)
        {
            if (expectThrow)
            {
                var lowerLeft = new Point3Int(llx, lly, llz);
                var upperRight = new Point3Int(urx, ury, urz);
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentOutOfRangeException>(() => new BoundingCuboid(lowerLeft, upperRight));
            }
            else
            {
                AssertValuesHold(llx, lly, llz, urx, ury, urz);
            }
        }

        [TestCase(0, 0, 0,true,TestName = "Inside")]
        [TestCase(-3, 0, 0, false, TestName = "Left of")]
        [TestCase(3, 0, 0, false, TestName = "Right of")]
        [TestCase(0, 3, 0, false, TestName = "Above")]
        [TestCase(0, -3, 0, false, TestName = "Below")]
        [TestCase(0, 0, 3, false, TestName = "InFront")]
        [TestCase(0, 0, -3, false, TestName = "Behind")]
        [TestCase(-1, 0, 0, true, TestName = "= left side")]
        [TestCase(0, -1, 0, true, TestName = "= bottom side")]
        [TestCase(0, 0, -1, true, TestName = "= front side")]
        [TestCase(1, 0, 0, false, TestName = "= right side")]
        [TestCase(0, 1, 0, false, TestName = "= top side")]
        [TestCase(0, 0, 1, false, TestName = "= back side")]
        public void ContainsPointCases(int x, int y, int z, bool shouldPass)
        {
            var point = new Point3Int(x, y, z);
            var box = new BoundingCuboid(new Point3Int(-1, -1, -1), new Point3Int(1, 1, 1));

            Assert.AreEqual(shouldPass, box.ContainsPoint(point));
        }

        [TestCase(-2,-2,-2,2,2,2,true,TestName = "Same size")]
        [TestCase(-3, -3, -3, 3, 3, 3, true, TestName = "Larger Encloses")]
        [TestCase(-1,-1,-1,1,1,1, true, TestName = "Smaller Inside")]

        [TestCase(-5,-1,-1,-4,1,1,false,TestName = "To left")]
        [TestCase(4, -1, -1, 5, 1, 1, false, TestName = "To right")]
        [TestCase(-1, -5, -1, 1, -4, 1, false, TestName = "below")]
        [TestCase(-1, 4, -1, 1, 5, 1, false, TestName = "above")]
        [TestCase(-1, -1, -5, 1, 1, -4, false, TestName = "in front")]
        [TestCase(-1, -1, 4, 1, 1, 5, false, TestName = "behind")]

        [TestCase(-3, -3, -3, -1, -1, -1, true, TestName = "intersect bottom left front")]
        [TestCase(1, -3, -3, 3, -1, -1, true, TestName = "intersect bottom right front")]
        [TestCase(-3, 1, -3, -1, 3, -1, true, TestName = "intersect bottom left back")]
        [TestCase(1, -3, -3, 3, -1, -1, true, TestName = "intersect bottom right back")]

        [TestCase(-3, -3, 1, -1, -1, 3, true, TestName = "intersect top left front")]
        [TestCase(1, -3, 1, 3, -1, 3, true, TestName = "intersect top right front")]
        [TestCase(-3, 1, 1, -1, 3, 3, true, TestName = "intersect top left back")]
        [TestCase(1, 1, 1, 3, 3, 3, true, TestName = "intersect top right back")]
        public void BoundaryTests(int minx, int miny, int minz, int maxx, int maxy, int maxz, bool shouldPass)
        {
            var bottomLeft = new Point3Int(minx, miny, minz);
            var upperRight = new Point3Int(maxx, maxy, maxz);

            var ut = new BoundingCuboid(bottomLeft, upperRight);

            var box = new BoundingCuboid(new Point3Int(-2, -2, -2), new Point3Int(2, 2, 2));

            Assert.AreEqual(shouldPass, box.DoesBoundaryCuboidIntersect(ut));
        }

        [TestCase(-1, -1, -1, 1, 1, 1, 0, 0, 0, true)]
        [TestCase(-3, -3, -3, -1, -1, -1, -2, -2, -2, true)]
        public void CenterTests(
            int minx, int miny, int minz,
            int maxx, int maxy, int maxz, 
            int x, int y, int z, 
            bool expected)
        {
            var ut = new BoundingCuboid(new Point3Int(minx, miny, minz), new Point3Int(maxx, maxy, maxz));
            var point = new Point3Int(x, y, z);
            var center = ut.CenterPoint();
            Assert.AreEqual(expected, point == center);
        }
    }
}
