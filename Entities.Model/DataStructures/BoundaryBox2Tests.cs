using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Util;
using Entities.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Entities.Model.DataStructures
{
    [TestFixture]
    class BoundaryBox2Tests
    {
        [TestCase()]
        public void AssertEmptyValuesAre0()
        {
            var ut = Bounding2DBox.Empty;
            Assert.AreEqual(0, ut.LowerLeft.X);
            Assert.AreEqual(0, ut.LowerLeft.Y);
            Assert.AreEqual(0, ut.UpperRight.X);
            Assert.AreEqual(0, ut.UpperRight.Y);
        }

        [TestCase()]
        public void AssertDefaultValuesAreEmpty()
        {
            var ut = new Bounding2DBox();
            Assert.AreEqual(Bounding2DBox.Empty, ut);
        }

        [TestCase(123, 134, 567, 567)]
        public void AssertValuesHold(int lx, int ly, int ux, int uy)
        {
            var bottomLeft = new Point2Int(lx, ly);
            var topRight = new Point2Int(ux, uy);
            var ut = new Bounding2DBox(bottomLeft, topRight);

            Assert.AreEqual(bottomLeft, ut.LowerLeft);
            Assert.AreEqual(topRight, ut.UpperRight);
        }

        [TestCase(1, 1, -1, -1, true)]
        [TestCase(-1, -1, 1, 1, false)]
        [TestCase(0, 0, 0, 0, false)]
        public void AssertThatBottomLeftisBottomLeftOfTopRight(int lx, int ly, int ux, int uy, bool expectThrow)
        {
            if (expectThrow)
            {
                var bottomLeft = new Point2Int(lx, ly);
                var topRight = new Point2Int(ux, uy);

                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentOutOfRangeException>(() => new Bounding2DBox(bottomLeft, topRight));
            }
            else
            {
                AssertValuesHold(lx, ly, ux, uy);
            }
        }

        [TestCase(-3, 0, false)]
        [TestCase(3, 0, false)]
        [TestCase(0, -3, false)]
        [TestCase(0, 3, false)]
        [TestCase(1, 0, false, TestName = "right edge check is less than")]
        [TestCase(1, 1, false, TestName = "right + top edge check is less than")]
        [TestCase(0, 1, false, TestName = "top edge check is less than")]
        [TestCase(-1, 0, true)]
        [TestCase(0, -1, true)]
        [TestCase(-1, -1, true)]
        public void ContainsCases(int x, int y, bool shouldPass)
        {
            var point = new Point2Int(x, y);
            var box = new Bounding2DBox(new Point2Int(-1, -1), new Point2Int(1, 1));

            Assert.AreEqual(shouldPass, box.ContainsPoint(point));
        }

        [TestCase(-3, -1, -2, 1, false, TestName = "box to left")]
        [TestCase(2, -1, 3, 1, false, TestName = "box to right")]
        [TestCase(-1, 2, 1, 3, false, TestName = "Box above")]
        [TestCase(-1, -3, 1, -2, false, TestName = "Box below")]
        [TestCase(-2, -2, 0, 0, true, TestName = "Box intersect bottom left")]
        [TestCase(-2, 0, 0, 1, true, TestName = "Box intersect top left")]
        [TestCase(0, -2, 2, 0, true, TestName = "Box intersect bottom right")]
        [TestCase(0, 0, 2, 2, true, TestName = "Box intersect top right")]
        [TestCase(-1, -1, 1, 1, true, TestName = "Box same size")]
        [TestCase(-2, -2, 2, 2, true, TestName = "Box larger")]
        [TestCase(-1, -1, 0, 0, true, TestName = "Box inside")]
        public void BoundaryTests(int minx, int miny, int maxx, int maxy, bool shouldPass)
        {
            var bottomLeft = new Point2Int(minx, miny);
            var topRight = new Point2Int(maxx, maxy);

            var areaUnderTest = new Bounding2DBox(bottomLeft, topRight);
            var box = new Bounding2DBox(new Point2Int(-1, -1), new Point2Int(1, 1));

            Assert.AreEqual(shouldPass, box.DoesBoundaryBoxIntersect(areaUnderTest));
        }
    }
}
