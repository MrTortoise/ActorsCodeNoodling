using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using NUnit.Framework;

namespace Entities.Model.Locations
{
    [TestFixture]
    public class LocationNameGeneratorTests
    {
        [TestCase()]
        public void Generate10000000NamesEnsureNoneAreDuplicates()
        {
            var testKit = new TestKit();
            var nameGenerator = testKit.ActorOfAsTestActorRef<World>();

        }
    }

    public class World : ReceiveActor 
    {
      
    }
}
