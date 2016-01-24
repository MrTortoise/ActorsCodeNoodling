using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.UniverseGenerator;
using NUnit.Framework;

namespace Entities.Model.UniverseGeneratorTests
{
    [TestFixture]
    class CdfTests
    {
        private static IEnumerable Histograms
        {
            get
            {
                yield return new TestCaseData(EmptyTupleArray,null).SetName("EmptyInput").Throws(typeof(ArgumentException));
                
            }
        }

        public static Tuple<double, int>[] EmptyTupleArray => new Tuple<double, int>[0];


        [Test, TestCaseSource("Histograms")]
        public void GenerteFromHistogram(Tuple<double, int>[] input, Tuple<double, int>[] valuesToProbe)
        {
            var graph = Cdf.GenerateFromHistogram(input);

            foreach (var tuple in valuesToProbe)
            {
                Assert.AreEqual(tuple.Item2, graph.F(tuple.Item1));
            }
        }
    }
}
