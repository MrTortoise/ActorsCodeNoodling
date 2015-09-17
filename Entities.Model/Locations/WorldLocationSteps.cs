using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.TestKit;
using Akka.Util.Internal;
using Entities.Model.ResourceManagerFeature;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.Locations
{
    [Binding]
    public class WorldLocationSteps
    {
        public ScenarioContextState State { get; }

        public WorldLocationSteps(ScenarioContextState state)
        {
            State = state;
        }

        [Given(@"I create a WorldPrefixPersistanceActor Actor")]
        public void GivenICreateAWorldPrefixPersistanceActorActor()
        {
            State.WorldPrefixPersistanceActor = State.TestKit.ActorOfAsTestActorRef<WorldPrefixPersistanceActor>();
        }
        
        [Given(@"I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state using test probe ""(.*)""")]
        public void GivenICreateTheFollowingPrefixesInTheWorldPrefixPersistanceActorActorAndStoreItsState(string probe,Table table)
        {
            var pr = State.TestProbes[probe];
            var prefixes = new List<string>(table.Rows.Select(r => r[0]));

            prefixes.ForEach(p => State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(p)));

            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage(), pr);
            pr.ExpectMsg<WorldPrefixPersistanceActor.StateSavedMessage>();
        }
        
        [When(@"I kill and restore the WorldPrefixPersistanceActor Actor")]
        public void WhenIKillAndRestoreTheWorldPrefixPersistanceActorActor()
        {
            Debug.WriteLine("about to watch World persistence actor");
            State.TestKit.Watch(State.WorldPrefixPersistanceActor);
            Debug.WriteLine("sending poison pill");
            State.WorldPrefixPersistanceActor.Tell(PoisonPill.Instance);
            Debug.WriteLine("Expecting terminated");
            State.TestKit.ExpectMsg<Terminated>();
            Debug.WriteLine("Terminated");


            //State.TestKit.Sys.Shutdown();
            //var man = new ResourceManagerSteps(State);
            //man.GivenICreateATestActorSystem();


            GivenICreateAWorldPrefixPersistanceActorActor();
        }

        [Then(@"I expect querying the WorldPrefixPersistanceActor with TestProbe ""(.*)"" to yield the following prefixes")]
        public void ThenIExpectQueryingTheWorldPrefixPersistanceActorWithTestProbeToYieldTheFollowingPrefixes(string name, Table table)
        {
            var probe = State.TestProbes[name];
            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.QueryPrefixes(), probe);

            var result = probe.ExpectMsg<WorldPrefixPersistanceActor.PostQueryResultsMessage>(TimeSpan.FromMilliseconds(5000));
            var prefixes = result.Prefixes;
            Assert.IsNotNull(prefixes);

            table.Rows.Select(r=>r[0]).ForEach(p =>
            {
                Assert.IsTrue(prefixes.Contains(p), $"prefixes.Contains({p})");
            });
        }

    }
}
