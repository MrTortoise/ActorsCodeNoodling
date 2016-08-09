using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NUnit.Framework;

namespace Entities.Model.ImmutableTests
{
    [TestFixture]
    class ImmutableDictionaryTest
    {
        [TestCase()]
        public void EqualityOfDifferentButEqualInstancesFails()
        {
            var test1 = "test1";
            var test2 = "test2";

            var aBuilder = ImmutableDictionary.CreateBuilder<string, string>();
            aBuilder.Add(test1, test1);
            aBuilder.Add(test2, test2);
            var a = aBuilder.ToImmutable();

            var bBuilder = ImmutableDictionary.CreateBuilder<string, string>();
            aBuilder.Add(test1, test1);
            aBuilder.Add(test2, test2);
            var b = bBuilder.ToImmutable();

            Assert.That(!a.Equals(b));
        }

        [TestCase()]
        public void EqualityOfDifferentButEqualInstancesSameKvpFails()
        {
            var test1 = "test1";
            var test2 = "test2";

            var kvp1 = new KeyValuePair<string, string>(test1, test1);
            var kvp2 = new KeyValuePair<string, string>(test2, test2);

            var aBuilder = ImmutableDictionary.CreateBuilder<string, string>();
            
            aBuilder.Add(kvp1);
            aBuilder.Add(kvp2);
            var a = aBuilder.ToImmutable();

            var bBuilder = ImmutableDictionary.CreateBuilder<string, string>();
            aBuilder.Add(kvp1);
            aBuilder.Add(kvp2);
            var b = bBuilder.ToImmutable();

            Assert.That(!a.Equals(b));
        }

        [TestCase(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
        [TestCase(new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 })]
        [TestCase(new[] {2,5,3,7,9,1,8,6})]
        public void TestLowToHighOnEnumerateValues(int[] values)
        {
            ImmutableDictionary<int, int> dic = ImmutableDictionary<int, int>.Empty;

            foreach (var value in values)
            {
                dic = dic.Add(value, value);
            }

            Array.Sort(values);

            var dicValues = dic.Values.ToArray();
            for (var i = 0; i < dicValues.Length; i++)
            {
                Assert.AreEqual(values[i], dicValues[i]);
            }
        }
    }
}
