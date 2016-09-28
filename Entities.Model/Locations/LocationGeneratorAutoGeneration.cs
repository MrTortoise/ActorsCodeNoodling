using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Entities.NameGenerators;
using Entities.Observation;
using Entities.RNG;
using NUnit.Framework;
using Serilog;

namespace Entities.Model.Locations
{
    [TestFixture]
    public class LocationGeneratorAutoGeneration : IDisposable
    {
        private TestKit _testkit;
        private IActorRef _random;
        private IActorRef _locationGenerator;

        [TearDown]
        public void TearDown()
        {
          
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
            _locationGenerator = _testkit.Sys.ActorOf(LocationNameGeneratorActor.CreateProps(_random, 6),
                LocationNameGeneratorActor.Name);
        }

        [TestCase()]
        [Ignore("because")]
        public void Generate100KLocations()
        {
            var tp = _testkit.CreateTestProbe("genTest");
            _locationGenerator.Tell(new Observe(), tp);
            _locationGenerator.Tell(new LocationNameGeneratorActor.GenerateLocationNames(10000), tp);

            var msg = tp.ExpectMsg<LocationNameGeneratorActor.LocationNamesAdded>(TimeSpan.FromMinutes(10));

            Assert.That(msg.AddedLocations.Length == 10000);

            var rnd = _testkit.CreateTestProbe("rnd");
            var lg = _testkit.CreateTestProbe("lg");
            rnd.Watch(_random);
            lg.Watch(_locationGenerator);

            _random.Tell(PoisonPill.Instance);
            _locationGenerator.Tell(PoisonPill.Instance);

            rnd.ExpectMsg<Terminated>();
            lg.ExpectMsg<Terminated>();

            _testkit.Shutdown(TimeSpan.FromSeconds(20), true);
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
