using System.Linq;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Entities.RNG;
using Entities.UniverseGenerator;
using NUnit.Framework;

namespace Entities.Model.UniverseGeneratorTests
{
    public class CdfGenerationTest : TestKit
    {
        [TestCase(100,0,100)]
        public void EqualityLineCdfTest(int noPointsToSample,int min, int max)
        {
            string pdfName = "equality";
            string cdfname = "cdfName";
            ISingleVariableFunction<double, int> function = new EqualityPdf();
            var randomActor = Sys.ActorOf(RandomDoubleActor.CreateProps(new System.Random()));
            var generator = Sys.ActorOf(DistributionGenerator.CreateProps(randomActor), DistributionGenerator.Name);

            var tp = CreateTestProbe();
            generator.Tell(new DistributionGenerator.SubscribeToPdfAdded());
            generator.Tell(new DistributionGenerator.AddProbabilityDensityFunction(pdfName, function), tp);

            var pdfAdded = tp.ExpectMsg<DistributionGenerator.PdfFunctionAdded>();


            generator.Tell(new DistributionGenerator.GenerateCdfFromPdf(cdfname, pdfName, min, max, noPointsToSample));
            generator.Tell(new DistributionGenerator.SubscribeToCdfAdded(), tp);

            var cdfAdded = tp.ExpectMsg<DistributionGenerator.CdfFunctionAdded>();


            string distributionName = "testDistribution";

            generator.Tell(new DistributionGenerator.Generate(distributionName, cdfname, noPointsToSample));
            generator.Tell(new DistributionGenerator.SubscribeToDistributionGenerated(), tp);

            var generatedMessage = tp.ExpectMsg<DistributionGenerator.DistributionGenerated>();

            Assert.AreEqual(distributionName, generatedMessage.DistributionName);
            Assert.IsTrue(generatedMessage.Distribution.Min() >= min);
            Assert.IsTrue(generatedMessage.Distribution.Max() <= max);
            Assert.AreEqual(noPointsToSample, generatedMessage.Distribution.Length);
            // * generate histogram
            // take a function of a line
            // take number of points we want to sample, say 10,000
            // take the min max ranges and generate input data and cumulate the total sum

            // * generate cdf
            // iterate again and add value of previous item to current
            // for each data divide by total area to see its cumulative contribution
            // iterate over again

            //


        }
    }
}