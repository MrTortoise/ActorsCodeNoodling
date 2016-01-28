using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Dispatch.SysMsg;
using Akka.TestKit;
using Akka.Util.Internal;
using Entities.Model.ResourceManagerFeature;
using Entities.NameGenerators;
using Entities.Observation;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.Locations
{
    [Binding]
    public class WorldLocationSteps
    {
        private const string WorldPersistenceActor = "worldPersistenceActor";
        public ScenarioContextState State { get; }

        public WorldLocationSteps(ScenarioContextState state)
        {
            State = state;
        }

        [Scope(Tag = "Persistence")]
        [Given(@"I create a WorldPrefixPersistanceActor Actor using testProbe ""(.*)""")]
        [When(@"I create a WorldPrefixPersistanceActor Actor using testProbe ""(.*)""")]
        public void GivenICreateAWorldPrefixPersistanceActorActorUsingTestProbe(string probe)
        {
            var testProbe = State.TestProbes[probe];
            State.Actors[WorldPersistenceActor] =
                testProbe.ActorOfAsTestActorRef<WorldPrefixPersistanceActor>(
                    Props.Create(() => new WorldPrefixPersistanceActor()), testProbe, WorldPersistenceActor);
          
            testProbe.ExpectMsg<WorldPrefixPersistanceActor.WorldPrefixPersistentRecoveryComplete>();

        }

        [Scope(Tag = "Persistence")]
        [Given(@"I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state using test probe ""(.*)""")]
        public void GivenICreateTheFollowingPrefixesInTheWorldPrefixPersistanceActorActorAndStoreItsState(string probe,Table table)
        {
            var pr = State.TestProbes[probe];
            var prefixes = new List<string>(table.Rows.Select(r => r[0]));
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            Assert.IsNotNull(persistenceActor);

            prefixes.ForEach(p =>
            {
                persistenceActor.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(p));
                Thread.Sleep(10);
                var msg = pr.ExpectMsg<WorldPrefixPersistanceActor.PrefixPersisted>(persisted => persisted.Prefix == p);
            });

            persistenceActor.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage(), pr);
            pr.ExpectMsg<WorldPrefixPersistanceActor.StateSavedMessage>();
        }

        [Scope(Tag = "Persistence")]
        [When(@"I kill the WorldPrefixPersistanceActor Actor")]
        public void WhenIKillAndRestoreTheWorldPrefixPersistanceActorActor()
        {
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            Debug.WriteLine("about to watch World persistence actor");
            State.TestKit.Watch(persistenceActor);
            Debug.WriteLine("sending poison pill");
            persistenceActor.Tell(PoisonPill.Instance);
            Debug.WriteLine("Expecting terminated");
            State.TestKit.ExpectMsg<Terminated>();
            Debug.WriteLine("Terminated");
        }

        [Scope(Tag = "Persistence")]
        [Then(@"I expect querying the WorldPrefixPersistanceActor with TestProbe ""(.*)"" to yield the following prefixes")]
        public void ThenIExpectQueryingTheWorldPrefixPersistanceActorWithTestProbeToYieldTheFollowingPrefixes(string name, Table table)
        {
            var probe = State.TestProbes[name];
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            persistenceActor.Tell(new WorldPrefixPersistanceActor.QueryPrefixes(null), probe);

            var result = probe.ExpectMsg<WorldPrefixPersistanceActor.PostQueryResultsMessage>(TimeSpan.FromMilliseconds(5000));
            var prefixes = result.Prefixes;
            Thread.Sleep(1);
            Assert.IsNotNull(prefixes);

            table.Rows.Select(r=>r[0]).ForEach(p =>
            {
                Assert.IsTrue(prefixes.Contains(p), $"prefixes.Contains({p})");
            });
        }

        [When(@"I add a location using ""(.*)"" called ""(.*)""")]
        public void WhenIAddALocationUsingCalled(string testProbe , string location)
        {
            var sender = State.TestProbes[testProbe];
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            persistenceActor.Tell(new LocationNameGeneratorActor.AddLocationName(new[] {location}), sender);
            Thread.Sleep(250);
        }

        [Given(@"I observe LocationGenerator with TestProbe ""(.*)""")]
        public void GivenIObserveLocationGeneratorWithTestProbe(string testProbe)
        {
            var observer = State.TestProbes[testProbe];
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            persistenceActor.Tell(new Observe(), observer);
        }


        [Then(@"I expect the location ""(.*)"" to exist")]
        public void ThenIExpectTheLocationToExist(string p0)
        {
            IActorRef persistenceActor = State.Actors[WorldPersistenceActor];
            var res = persistenceActor.Ask<LocationNameGeneratorActor.LocationNamesResult>(new LocationNameGeneratorActor.QueryLocationNames());
            res.Wait();
            Assert.AreEqual(1,res.Result.Names.Count());
            Assert.AreEqual(p0,res.Result.Names.Single());
        }

        [When(@"I poison the LocationGenerator")]
        public void WhenIPoisonTheLocationGenerator()
        {
            Debug.WriteLine("about to watch World persistence actor");
            State.TestKit.Watch(RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef);
            
            Debug.WriteLine("sending poison pill");
            RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef.Tell(PoisonPill.Instance);
            Thread.Sleep(1);
            Debug.WriteLine("Expecting terminated");
            var msg = State.TestKit.ExpectMsg<Terminated>();
            Thread.Sleep(1);
            State.TestKit.Unwatch(RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef);
            Debug.WriteLine("Terminated");
           // Thread.Sleep(5000);

            // GivenICreateAWorldPrefixPersistanceActorActor();
        }

        [Then(@"I expect that TestProbe ""(.*)"" be told the following locations was added ""(.*)""")]
        public void ThenIExpectThatTestProbeBeToldTheFollowingLocationsWasAdded(string testProbe, string location)
        {
            var probe = State.TestProbes[testProbe];
            var msg = probe.ExpectMsg<LocationNameGeneratorActor.LocationNamesAdded>();
            Assert.Contains(location, msg.AddedLocations);
        }

        [When(@"I add the following locations using ""(.*)""")]
        public void WhenIAddTheFollowingLocationsUsing(string probeName, Table table)
        {
            var locations = GetLocations(table);
            var probe = State.TestProbes[probeName];
            RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef.Tell(new LocationNameGeneratorActor.AddLocationName(locations), probe);
        }

        private static string[] GetLocations(Table table)
        {
            return table.Rows.Select(i => i[0]).ToArray();
        }

        [Then(@"I expect that TestProbe ""(.*)"" be told the following locations were added")]
        public void ThenIExpectThatTestProbeBeToldTheFollowingLocationsWereAdded(string probeName, Table table)
        {
            var locations = GetLocations(table);
            var probe = State.TestProbes[probeName];
            var msg = probe.ExpectMsg<LocationNameGeneratorActor.LocationNamesAdded>();
            foreach (var location in locations)
            {
                Assert.Contains(location, msg.AddedLocations);
            }
        }

        [Then(@"I expect the follwing locations to exist")]
        public void ThenIExpectTheFollwingLocationsToExist(Table table)
        {
            var locations = GetLocations(table);

            var res = RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef.Ask<LocationNameGeneratorActor.LocationNamesResult>(new LocationNameGeneratorActor.QueryLocationNames());
            res.Wait();
            
            foreach (var location in locations)
            {
                Assert.Contains(location,res.Result.Names);
            }
        }
    }
}
