using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Entities.RNG;
using NUnit.Framework;
using Serilog;

namespace Entities.Model.Random
{
    [TestFixture]
    public class RandomTests : IDisposable
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
               .WriteTo.LiterateConsole()
               .MinimumLevel.Debug()
               .CreateLogger();
            Serilog.Log.Logger = logger;

            var config = "akka { loglevel=DEBUG,  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}";

            _testkit = new TestKit(config, "testSystem");
            var random = new System.Random();
            _random = _testkit.Sys.ActorOf(RandomIntActor.CreateProps(random), "random");
        }

        /// <summary>
        /// Takes freaking ages for 1 million though.
        /// </summary>
        /// <remarks>
        /// But if you want like 1 million then why use random? 
        /// 3 chars gives 17576 then use 2 digits and we have 1.7M
        /// So can probalby then just manually trim out any that look crap
        /// </remarks>
        [TestCase()]
        public void GeneralBoundsTests()
        {
            var minValue = 1;
            var maxValue = 12345;
            var numberOfNumbers = 1000;

            var msg = _random.Ask<RandomIntActor.RandomResult>(new RandomIntActor.NextRandom(minValue, maxValue, numberOfNumbers));
            msg.Wait();

            Assert.That(msg.Result.Number.Length == numberOfNumbers);
            Assert.That(msg.Result.Number.Min() >= minValue);
            Assert.That(msg.Result.Number.Max() <= maxValue);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _testkit.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RandomTests() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
