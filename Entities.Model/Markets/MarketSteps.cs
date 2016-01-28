using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.NameGenerators;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.Markets
{
    [Binding]
    public class MarketSteps
    {
        private readonly ScenarioContextState _state;

        public MarketSteps(ScenarioContextState state)
        {
            _state = state;
        }



        [When(@"I create the following markets using testProbe ""(.*)""")]
        public void WhenICreateTheFollowingMarketsUsingTestProbe(string creatorName, Table table)
        {
            var marketHub = RootLevelActors.MarketHubActorRef;
            var creator = _state.TestProbes[creatorName];
            table.Rows.ForEach(r =>
            {
                var name = r["Name"];

                RootLevelActors.GeneratorActors.LocationNameGeneratorActorRef.Tell(new LocationNameGeneratorActor.AddLocationName(new[] {r["Location"]}));
                var location = r["Location"];

                //ToDo unfook
              //  var createMarketMessage = new MarketHub.TellCreateMarketMessage(name, location);
              //  marketHub.Tell(createMarketMessage, creator);
            });
        }

        [Then(@"I expect testProbe ""(.*)"" of been notified of the market having been created")]
        public void ThenIExpectTestProbeOfBeenNotifiedOfTheMarketHavingBeenCreated(string probeName, Table table)
        {
            var testProbe = _state.TestProbes[probeName];
            Assert.IsTrue(testProbe.HasMessages);

            var message = testProbe.ExpectMsg<MarketHub.TellMarketCreatedMessage>(TimeSpan.FromMilliseconds(500));
            Assert.IsNotNull(message);

            _state.Markets.Add(message.MarketActor.Path.Name, message.MarketActor);
        }

        [Then(@"I expect to see the following markets when I query the listings")]
        public void ThenIExpectToSeeTheFollowingmarketsWhenIQueryTheListings(Table table)
        {
            var marketQueryTask =
                RootLevelActors.MarketHubActorRef.Ask<MarketHub.ResultsMarketListings>(
                    new MarketHub.QueryMarketListingsMessage(), TimeSpan.FromMilliseconds(500));

            marketQueryTask.Wait(500);
            Assert.IsTrue(marketQueryTask.IsCompleted);

            var names = new List<string>();
            table.Rows.ForEach(r =>
            {
                names.Add(r["Name"]);
            });

            var result = marketQueryTask.Result;
            result.MarketListings.ForEach(i => Assert.IsTrue(names.Contains(i.MarketName)));
        }

    }
}
