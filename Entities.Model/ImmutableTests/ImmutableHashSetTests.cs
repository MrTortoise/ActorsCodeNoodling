using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Entities.Model.ImmutableTests
{
    [TestFixture]
    public class ImmutableHashSetTests
    {
        [TestCase(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}, TestName = "ascending")]
        [TestCase(new[] {9, 8, 7, 6, 5, 4, 3, 2, 1}, TestName = "descending")]
        [TestCase(new[] {2, 9, 3, 8, 4, 7, 5, 6}, TestName = "random")]
        public void DoesImmutableHashsetEnumerateAscending(int[] values)
        {
            ImmutableHashSet<int> hashSet = ImmutableHashSet<int>.Empty;

            foreach (var value in values)
            {
                hashSet = hashSet.Add(value);
            }

            Array.Sort(values);

            int i = 0;
            foreach (var val in hashSet)
            {
                Assert.AreEqual(values[i], val);
                i++;
            }
        }
    }
}
