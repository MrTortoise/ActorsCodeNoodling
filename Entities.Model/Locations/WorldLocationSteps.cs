using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.Util.Internal;
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
        
        [Given(@"I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state")]
        public void GivenICreateTheFollowingPrefixesInTheWorldPrefixPersistanceActorActorAndStoreItsState(Table table)
        {
            var prefixes = new List<string>(table.Rows.Select(r => r[0]));

            prefixes.ForEach(p => State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(p)));

            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage());
        }
        
        [When(@"I kill and restore the WorldPrefixPersistanceActor Actor")]
        public void WhenIKillAndRestoreTheWorldPrefixPersistanceActorActor()
        {
            State.WorldPrefixPersistanceActor.GracefulStop(TimeSpan.FromMilliseconds(500)).Wait();
            State.WorldPrefixPersistanceActor = null;
            State.WorldPrefixPersistanceActor = State.TestKit.ActorOfAsTestActorRef<WorldPrefixPersistanceActor>();
        }

        [Then(@"I expect querying the WorldPrefixPersistanceActor with TestProbe ""(.*)"" to yield the following prefixes")]
        public void ThenIExpectQueryingTheWorldPrefixPersistanceActorWithTestProbeToYieldTheFollowingPrefixes(string name, Table table)
        {
            var probe = State.TestProbes[name];
            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.QueryPrefixes(), probe);

            var result = probe.ExpectMsg<WorldPrefixPersistanceActor.PostQueryResultsMessage>(TimeSpan.FromMilliseconds(500));
            var prefixes = result.Prefixes;
            Assert.IsNotNull(prefixes);

            table.Rows.Select(r=>r[0]).ForEach(p =>
            {
                Assert.IsTrue(prefixes.Contains(p), $"prefixes.Contains({p})");
            });
        }

    }
}
