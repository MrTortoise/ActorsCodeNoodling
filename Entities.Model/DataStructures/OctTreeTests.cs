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
    public class OctTreeTests
    {
        [TestCase()]
        public void AddAPointAssertExists()
        {
            var ut = new OctTree(BoundingCuboid.Max, 10, new SimpleOctTreeDivisionStrategy());
            ut.Add(Point3Int.Zero);
        }
    }

 
}
