using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Entities.RNG;
using Entities.UniverseGenerator;
using NUnit.Framework;

namespace Entities.Model.UniverseGeneratorTests
{
    [TestFixture]
    class DistributionProviderTests : TestKit
    {
        [TestCase(0,10,20)]
        public void CallGeneratorWithMinMaxNoResults(int min, int max, int noResults)
        {
            string functionName = "equality";
            ISingleVariableFunction<int, double> function = new EqualityCdf(min, max);
            var randomActor = Sys.ActorOf(RandomDoubleActor.CreateProps(new System.Random()));
            var generator = Sys.ActorOf(DistributionGenerator.CreateProps(randomActor), DistributionGenerator.Name);

            var tp = CreateTestProbe();
            generator.Tell(new DistributionGenerator.SubscribeToCdfAdded(), tp);
            generator.Tell(new DistributionGenerator.AddCumulativeDistributionFunction(functionName, function), tp);

            var msg = tp.ExpectMsg<DistributionGenerator.CdfFunctionAdded>();
            Assert.AreEqual(functionName, msg.FunctionName);
            
            string distributionName = "testDistribution";

            generator.Tell(new DistributionGenerator.Generate(distributionName, functionName, noResults));
            generator.Tell(new DistributionGenerator.SubscribeToDistributionGenerated(), tp);

            var generatedMessage = tp.ExpectMsg<DistributionGenerator.DistributionGenerated>();

            Assert.AreEqual(distributionName, generatedMessage.DistributionName);
            Assert.IsTrue(generatedMessage.Distribution.Min() >= min);
            Assert.IsTrue(generatedMessage.Distribution.Max() <= max);
            Assert.AreEqual(noResults,generatedMessage.Distribution.Length);
        }
    }
}
