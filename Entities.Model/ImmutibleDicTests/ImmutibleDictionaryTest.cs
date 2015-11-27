using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Immutable;

namespace Entities.Model.ImmutibleDicTests
{
    [TestFixture]
    class ImmutibleDictionaryTest
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
    }
}
