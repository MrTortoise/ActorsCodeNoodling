using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.TestKit.NUnit;
using NUnit.Framework;

namespace Entities.Model.Akka.Testkit.Experiments
{
    [TestFixture]
    public class FsmBecomeTests
    {
        [TestCase()]
        public void InitialStateIsStart()
        {
            var testKit = new TestKit();
            var fsmActor = testKit.ActorOfAsTestActorRef<FsmBecomeActor>();
            Assert.AreEqual(FsmActorState.Start,fsmActor.UnderlyingActor.State);  
        }

        [TestCase()]
        public void AfterOfferIsOffered()
        {
            var testKit = new TestKit();
            var fsmActor = testKit.ActorOfAsTestActorRef<FsmBecomeActor>();
            fsmActor.Tell("offer");
            Assert.AreEqual(FsmActorState.Offered,fsmActor.UnderlyingActor.State);
        }

        [TestCase()]
        public void AfterOfferAcceptedIsAccepted()
        {
            var testKit = new TestKit();
            var fsmActor = testKit.ActorOfAsTestActorRef<FsmBecomeActor>();
            fsmActor.Tell("offer");
            fsmActor.Tell("accept");
            Assert.AreEqual(FsmActorState.Accepted,fsmActor.UnderlyingActor.State);
        }

        [TestCase()]
        public void AfterOfferRejectedIsStart()
        {
            var testKit = new TestKit();
            var fsmActor = testKit.ActorOfAsTestActorRef<FsmBecomeActor>();
            fsmActor.Tell("offer");
            fsmActor.Tell("reject");
            Assert.AreEqual(FsmActorState.Start, fsmActor.UnderlyingActor.State);
        }
    }

    public enum FsmActorState
    {
        Start,
        Offered,
        Accepted
    }
}
