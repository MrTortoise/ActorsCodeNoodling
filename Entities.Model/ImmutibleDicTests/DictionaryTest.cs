using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Entities.Model.ImmutibleDicTests
{
    [TestFixture]
    public class DictionaryTest
    {
        [TestCase()]
        public void EqualityOfDifferentButEqualInstancesFails()
        {
            var a = new Dictionary<string,string>();
            var b = new Dictionary<string, string>();

            var test1 = "test1";
            a.Add(test1,test1);

            var test2 = "test2";
            a.Add(test2, test2);

            b.Add(test1, test1);
            b.Add(test2, test2);

            Assert.That(!a.Equals(b));
        }
    }
}
