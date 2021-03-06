﻿using System;
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
                yield return new TestCaseData(SimpleDistribution, SimpleDistributionTestValues).SetName("Simple Linear");
            }
        }

        public static Tuple<double, int>[] EmptyTupleArray => new Tuple<double, int>[0];

        public static Tuple<double, int>[] NoZeroValue => new[] {new Tuple<double, int>(0.5, 10), new Tuple<double, int>(1, 20)};

        public static Tuple<double, int>[] NoOneValue => new[] {new Tuple<double, int>(0, 10), new Tuple<double, int>(0.5, 20)};

        public static Tuple<double, int>[] SimpleDistribution
        {
            get
            {
                var retVal = new Tuple<double, int>[11];
                for (int i = 0; i <= 10; i++)
                {
                    double boundary = i*0.1;
                    int value = i*10 + 10;
                    retVal[i] = new Tuple<double, int>(boundary, value);
                }

                return retVal;
            }
        }

        public static Tuple<double, int>[] SimpleDistributionTestValues
        {
            get
            {
                var retVal = new Tuple<double, int>[12];
                retVal[0] = new Tuple<double, int>(0, 10);
                retVal[1] = new Tuple<double, int>(0.05, 10);
                retVal[2] = new Tuple<double, int>(0.15, 20);
                retVal[3] = new Tuple<double, int>(0.25, 30);
                retVal[4] = new Tuple<double, int>(0.35, 40);
                retVal[5] = new Tuple<double, int>(0.45, 50);
                retVal[6] = new Tuple<double, int>(0.55, 60);
                retVal[7] = new Tuple<double, int>(0.65, 70);
                retVal[8] = new Tuple<double, int>(0.75, 80);
                retVal[9] = new Tuple<double, int>(0.85, 90);
                retVal[10] = new Tuple<double, int>(0.95, 100);
                retVal[11] = new Tuple<double, int>(1, 110);
                return retVal;
            }
        }

        [Test]
        public void EmptyInputFailsTest()
        {
            Assert.Throws<ArgumentException>(() => Cdf.GenerateFromHistogram(EmptyTupleArray));
        }

        [Test]
        public void NoZeroValueFailsTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Cdf.GenerateFromHistogram(NoZeroValue));
        }

        [Test]
        public void NoOneValueFailsTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Cdf.GenerateFromHistogram(NoOneValue));
        }

            


        [Test]
        public void GenerteFromHistogram()
        {
            var graph = Cdf.GenerateFromHistogram(SimpleDistribution);

            foreach (var tuple in SimpleDistributionTestValues)
            {
                int actual = graph.F(tuple.Item1);
                Assert.AreEqual(tuple.Item2, actual, $"F({(double)tuple.Item1})={actual} ... != {tuple.Item2}");
            }
        }
    }
}
