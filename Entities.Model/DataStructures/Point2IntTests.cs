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
    class Point2IntTests
    {
        [TestCase()]
        public void CreatePoint2IntEnsureValuesCorrect()
        {
            int x = 234;
            int y = -564;

            var ut = new Point2Int(x, y);

            Assert.AreEqual(x,ut.X);
            Assert.AreEqual(y,ut.Y);
        }

        [TestCase()]
        public void CheckEmptyValuesare0()
        {
            var ut = Point2Int.Zero;
            Assert.AreEqual(0,ut.X);
            Assert.AreEqual(0,ut.Y);
        }

        [TestCase()]
        public void CheckDefaultValuesareEmpty()
        {
            var ut = new Point2Int();
            Assert.AreEqual(Point2Int.Zero, ut);
        }
    }
}
