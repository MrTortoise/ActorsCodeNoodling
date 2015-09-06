using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Util.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [Given(@"I initialise the MarketHub Actor")]
        public void GivenIInitialiseTheMarketListingsActor()
        {
            _state.MarketHubActor = _state.TestKit.ActorOfAsTestActorRef<MarketHub>("TestActorHub");
        }

        [When(@"I create the following markets using testProbe ""(.*)""")]
        public void WhenICreateTheFollowingMarketsUsingTestProbe(string creatorName, Table table)
        {
            var marketHub = _state.MarketHubActor;
            var creator = _state.TestProbes[creatorName];
            table.Rows.ForEach(r =>
            {
                var name = r["Name"];
                var createMarketMessage = new MarketHub.TellCreateMarketMessage(name);
                marketHub.Tell(createMarketMessage, creator);
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
                _state.MarketHubActor.Ask<MarketHub.ResultsMarketListings>(
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
