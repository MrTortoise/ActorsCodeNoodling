using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using Serilog;

namespace Entities.Model.Random
{
    [TestFixture]
    public class RandomTests
    {
        private TestKit _testkit;
        private IActorRef _random;

        [TearDown]
        public void TearDown()
        {
            var tp = _testkit.CreateTestProbe("tp");
            tp.Watch(_random);
            _random.Tell(PoisonPill.Instance);
            tp.ExpectMsg<Terminated>();
            _testkit.Shutdown();
        }

        [SetUp]
        public void Setup()
        {
            var logger = new LoggerConfiguration()
               .WriteTo.ColoredConsole()
               .MinimumLevel.Debug()
               .CreateLogger();
            Serilog.Log.Logger = logger;

            var config = "akka { loglevel=DEBUG,  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}";

            _testkit = new TestKit(config, "testSystem");
            var random = new System.Random();
            _random = _testkit.Sys.ActorOf(RandomActor.CreateProps(random), "random");
        }

        [TestCase()]
        public void GeneralBoundsTests()
        {
            var minValue = 1;
            var maxValue = 12345;
            var numberOfNumbers = 1000000;

            var msg = _random.Ask<RandomActor.RandomResult>(new RandomActor.NextRandom(minValue, maxValue, numberOfNumbers));
            msg.Wait();

            Assert.That(msg.Result.Number.Length == numberOfNumbers);
            Assert.That(msg.Result.Number.Min() >= minValue);
            Assert.That(msg.Result.Number.Max() <= maxValue);
        }
    }
}
