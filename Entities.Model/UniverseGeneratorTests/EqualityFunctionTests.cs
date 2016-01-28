using System;
using Entities.UniverseGenerator;
using NUnit.Framework;

namespace Entities.Model.UniverseGeneratorTests
{
    [TestFixture]
    public class EqualityFunctionTests
    {
        [TestCase(2,1)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void EnsureMinLessThanEqualMax(int min, int max)
        {
            if (min > max)
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentOutOfRangeException>(() => new EqualityCdf(min, max));
            }
            else
            {
                var ut = new EqualityCdf(min, max);
                Assert.AreEqual(max, ut.Max);
                Assert.AreEqual(min, ut.Min);
            }
        }
    }
}