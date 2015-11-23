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
            State.WorldPrefixPersistanceActor =
                testProbe.ActorOfAsTestActorRef<WorldPrefixPersistanceActor>(
                    Props.Create(() => new WorldPrefixPersistanceActor()), testProbe, "worldPersistenceActor");
            var parent = State.WorldPrefixPersistanceActor.Path.Parent;
            testProbe.ExpectMsg<WorldPrefixPersistanceActor.WorldPrefixPersistentRecoveryComplete>();

        }

        [Scope(Tag = "Persistence")]
        [Given(@"I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state using test probe ""(.*)""")]
        public void GivenICreateTheFollowingPrefixesInTheWorldPrefixPersistanceActorActorAndStoreItsState(string probe,Table table)
        {
            var pr = State.TestProbes[probe];
            var prefixes = new List<string>(table.Rows.Select(r => r[0]));

            prefixes.ForEach(p =>
            {
                State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(p));
                Thread.Sleep(10);
                var msg = pr.ExpectMsg<WorldPrefixPersistanceActor.PrefixPersisted>(persisted => persisted.Prefix == p);
            });

            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage(), pr);
            pr.ExpectMsg<WorldPrefixPersistanceActor.StateSavedMessage>();
        }

        [Scope(Tag = "Persistence")]
        [When(@"I kill the WorldPrefixPersistanceActor Actor")]
        public void WhenIKillAndRestoreTheWorldPrefixPersistanceActorActor()
        {
            Debug.WriteLine("about to watch World persistence actor");
            State.TestKit.Watch(State.WorldPrefixPersistanceActor);
            Debug.WriteLine("sending poison pill");
            State.WorldPrefixPersistanceActor.Tell(PoisonPill.Instance);
            Debug.WriteLine("Expecting terminated");
            State.TestKit.ExpectMsg<Terminated>();
            Debug.WriteLine("Terminated");
        }

        [Scope(Tag = "Persistence")]
        [Then(@"I expect querying the WorldPrefixPersistanceActor with TestProbe ""(.*)"" to yield the following prefixes")]
        public void ThenIExpectQueryingTheWorldPrefixPersistanceActorWithTestProbeToYieldTheFollowingPrefixes(string name, Table table)
        {
            var probe = State.TestProbes[name];
            State.WorldPrefixPersistanceActor.Tell(new WorldPrefixPersistanceActor.QueryPrefixes(null), probe);

            var result = probe.ExpectMsg<WorldPrefixPersistanceActor.PostQueryResultsMessage>(TimeSpan.FromMilliseconds(5000));
            var prefixes = result.Prefixes;
            Thread.Sleep(1);
            Assert.IsNotNull(prefixes);

            table.Rows.Select(r=>r[0]).ForEach(p =>
            {
                Assert.IsTrue(prefixes.Contains(p), $"prefixes.Contains({p})");
            });
        }

        [When(@"I have created a LocationGenerator Actor")]
        [Given(@"I have created a LocationGenerator Actor")]
        public void GivenIHaveCreatedALocationGeneratorActor()
        {
            State.LocationGeneratorActor =
                State.TestKit.ActorOfAsTestActorRef<LocationNameGeneratorActor>(
                    LocationNameGeneratorActor.CreateProps(State.RandomActor, 3),
                    LocationNameGeneratorActor.Name);

            Thread.Sleep(250);
        }

        [When(@"I add a location using ""(.*)"" called ""(.*)""")]
        public void WhenIAddALocationUsingCalled(string testProbe , string location)
        {
            var sender = State.TestProbes[testProbe];
            State.LocationGeneratorActor.Tell(new LocationNameGeneratorActor.AddLocationName(new[] {location}), sender);
            Thread.Sleep(250);
        }

        [Given(@"I observe LocationGenerator with TestProbe ""(.*)""")]
        public void GivenIObserveLocationGeneratorWithTestProbe(string testProbe)
        {
            var observer = State.TestProbes[testProbe];
            State.LocationGeneratorActor.Tell(new Observe(), observer);
        }


        [Then(@"I expect the location ""(.*)"" to exist")]
        public void ThenIExpectTheLocationToExist(string p0)
        {
            var res = State.LocationGeneratorActor.Ask<LocationNameGeneratorActor.LocationNamesResult>(new LocationNameGeneratorActor.QueryLocationNames());
            res.Wait();
            Assert.AreEqual(1,res.Result.Names.Count());
            Assert.AreEqual(p0,res.Result.Names.Single());
        }

        [When(@"I poison the LocationGenerator")]
        public void WhenIPoisonTheLocationGenerator()
        {
            Debug.WriteLine("about to watch World persistence actor");
            State.TestKit.Watch(State.LocationGeneratorActor);
            Debug.WriteLine("sending poison pill");
            State.LocationGeneratorActor.Tell(PoisonPill.Instance);
            Thread.Sleep(1);
            Debug.WriteLine("Expecting terminated");
            var msg = State.TestKit.ExpectMsg<Terminated>();
            Thread.Sleep(1);
            State.TestKit.Unwatch(State.LocationGeneratorActor);
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
            State.LocationGeneratorActor.Tell(new LocationNameGeneratorActor.AddLocationName(locations), probe);
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

            var res = State.LocationGeneratorActor.Ask<LocationNameGeneratorActor.LocationNamesResult>(new LocationNameGeneratorActor.QueryLocationNames());
            res.Wait();
            
            foreach (var location in locations)
            {
                Assert.Contains(location,res.Result.Names);
            }
        }
    }
}
