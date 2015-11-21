using System;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TechTalk.SpecFlow;

namespace Entities.Model.ContractActors
{
    [Binding]
    public class ExchangeContractFeatureSteps
    {
        private readonly ScenarioContextState _state;

        public ExchangeContractFeatureSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I create an ExchangeContractActor called ""(.*)"" with supervisor TestProbe called ""(.*)""")]
        [When(@"I create an ExchangeContractActor called ""(.*)"" with supervisor TestProbe called ""(.*)""")]
        public void GivenICreateAnExchangeContractActorCalled(string name, string supervisorName)
        {
            var supervisor = _state.TestProbes[supervisorName];
            var exchangeContractActor = _state.TestKit.ActorOfAsTestActorRef<ExchangeContract>(supervisor, name);
            _state.ExchangeContractActors.Add(name, exchangeContractActor);
        }

        [Given(@"I have configured the DateTime provider to return ""(.*)""")]
        public void GivenIHaveConfiguredTheDateTimeProviderToReturn(string dateTimeString)
        {
            var dateTime = DateTime.Parse(dateTimeString);
            DateTimeProvider.SetProvider(new FixedDateTime(dateTime));
        }

        [Given(@"I have created a TestActor called ""(.*)""")]
        public void GivenIHaveCreatedATestActorCalled(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I post to the ExchangeContract ""(.*)"" the following invitation")]
        [When(@"I post to the ExchangeContract ""(.*)"" the following invitation")]
        public void WhenIPostToTheExchangeContractTheFollowingInvitation(string exchangeContractName, Table table)
        {
            var exchangeContractActor = _state.ExchangeContractActors[exchangeContractName];
            var exchangeType = (OfferType)Enum.Parse(typeof(OfferType), table.GetField("ExchangeType"));

            var resourceName = table.GetField("SellResourceName");
            var sellResource = _state.GetResourceFromName(resourceName);

            var sellResourceQuantity = int.Parse(table.GetField("SellResourceQuantity"));
            var sellResourceTimePeriod =
                (TimePeriodType)Enum.Parse(typeof(TimePeriodType), table.GetField("SellResourceTimePeriod"));
            var sellResourceTimePeriodQuantity = int.Parse(table.GetField("SellResourceTimePeriodQuantity"));

            var suggestedOfferResourceName = table.GetField("SuggestedOfferResourceName");
            var suggestedOfferResource = _state.GetResourceFromName(suggestedOfferResourceName);
            var suggestedQuantity = int.Parse(table.GetField("SuggestedOfferResourceQuantity"));

            var liabilityResourceName = table.GetField("LiabilityResourceName");
            var liabilityResource = _state.GetResourceFromName(liabilityResourceName);
            var liabilityQuantity = int.Parse(table.GetField("LiabilityResourceQuantity"));

            var owner = table.GetField("ContractOwner");
            var contractOwner = owner == "testActor" ? _state.TestKit.TestActor : _state.Traders[owner];

            var postInvitationMessage = new ExchangeContract.PostInvitationMessage(
                exchangeType,
                sellResource,
                sellResourceQuantity,
                sellResourceTimePeriod,
                sellResourceTimePeriodQuantity,
                suggestedOfferResource,
                suggestedQuantity,
                liabilityResource,
                liabilityQuantity);

            exchangeContractActor.Tell(postInvitationMessage, contractOwner);
        }

        [Then(@"I expect the state of the ExchangeContractActor ""(.*)"" to be ""(.*)""")]
        public void ThenIExpectTheStateOfTheExchangeContractActorToBe(string exchangeContractActorName,
            string exchangeContractActorState)
        {
            var exchangeActorRef = _state.ExchangeContractActors[exchangeContractActorName];
            var stateTask = exchangeActorRef.Ask<ExchangeContract.State>(new ExchangeContract.QueryStateMessage(),
                TimeSpan.FromMilliseconds(50));
            stateTask.Wait();
            var expectedState =
                (ExchangeContract.State)Enum.Parse(typeof(ExchangeContract.State), exchangeContractActorState);

            Assert.AreEqual(expectedState, stateTask.Result);
        }

        [Then(@"I expect the creator of ExchangeContractActor ""(.*)"" to be ""(.*)""")]
        public void ThenIExpectTheCreatorOfExchangeContractActorToBe(string exchangeContractActorName, string creator)
        {
            var exchangeActorRef = _state.ExchangeContractActors[exchangeContractActorName];
            var creatorActorRef = _state.Traders[creator];

            var ownerTask = exchangeActorRef.Ask<IActorRef>(new ExchangeContract.QuerySeller(),
                TimeSpan.FromMilliseconds(50));
            ownerTask.Wait();
            var owner = ownerTask.Result;

            Assert.AreEqual(creatorActorRef, owner);
        }

        [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following for offer")]
        public void ThenIExpectTheExchangeContractActorToHaveTheFollowingForOffer(string exchangeContract, Table table)
        {
            var exchangeActorRef = _state.ExchangeContractActors[exchangeContract];
            var queryTask = exchangeActorRef.Ask<InvitationToTreat>(new ExchangeContract.QueryInvitationToTreat(),
                TimeSpan.FromMilliseconds(50));

            queryTask.Wait();
            var invitation = queryTask.Result;

            var exchangeType = Enum.Parse(typeof(OfferType), table.GetField("ExchangeType"));
            var resource = _state.GetResourceFromName(table.GetField("Resource"));
            var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("Quantity")));
            var dateTime = DateTime.Parse(table.GetField("CompletionTime"));

            resource = _state.GetResourceFromName(table.GetField("SuggestedOfferResourceName"));
            var suggestedResource = new ResourceStack(resource,
                int.Parse(table.GetField("SuggestedOfferResourceQuantity")));

            resource = _state.GetResourceFromName(table.GetField("LiabilityResourceName"));
            var liabilityResource = new ResourceStack(resource, int.Parse(table.GetField("LiabilityResourceQuantity")));

            Assert.AreEqual(exchangeType, invitation.ExchangeType);
            Assert.AreEqual(resourceStack, invitation.InvitationStack);
            Assert.AreEqual(dateTime, invitation.InvitationDeadline);
            Assert.AreEqual(suggestedResource, invitation.SuggestedOffer);
            Assert.AreEqual(liabilityResource, invitation.LiabilityStack);
        }

        [When(@"the Trader called ""(.*)"" makes the following offer on the ExchangeContractActor called ""(.*)""")]
        public void WhenTheTraderCalledMakesTheFollowingOfferOnTheExchangeContractActorCalled(string buyerName,
            string exchangeContractName, Table table)
        {
            var buyerActorRef = _state.Traders[buyerName];
            var exchangeActorRef = _state.ExchangeContractActors[exchangeContractName];

            var resourceStack = ConstructResourceStack(table);
            var liabilityResourceStack = ConstructLiabilityResourceStack(table);

            exchangeActorRef.Tell(new ExchangeContract.PostOffer(resourceStack, liabilityResourceStack), buyerActorRef);
        }

        private ResourceStack ConstructResourceStack(Table table)
        {
            var resource = _state.GetResourceFromName(table.GetField("Resource"));
            var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("Quantity")));
            return resourceStack;
        }

        private ResourceStack ConstructLiabilityResourceStack(Table table)
        {
            var resource = _state.GetResourceFromName(table.GetField("LiabilityResource"));
            var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("LiabilityQuantity")));
            return resourceStack;
        }

        [Then(@"I expect that the TestActor will of been notified of the following offer being made")]
        public void ThenIExpectThatTheTestActorWillOfBeenNotifiedOfTheFollowingOfferBeingMade(Table table)
        {
            var resourceStack = ConstructResourceStack(table);
            var liabilityResourceStack = ConstructLiabilityResourceStack(table);
            var sender = _state.Traders[table.GetField("SenderName")];

            var offer =
                _state.TestKit.ExpectMsg<ExchangeContract.OfferMadeNotification>(
                    (offerMade) => resourceStack.Resource.Equals(offerMade.Offer.ResourceStack.Resource)
                                   && resourceStack.Quantity == offerMade.Offer.ResourceStack.Quantity
                                   && liabilityResourceStack.Resource.Equals(offerMade.Offer.LiabilityStack.Resource)
                                   && liabilityResourceStack.Quantity == offerMade.Offer.LiabilityStack.Quantity
                                   && ReferenceEquals(sender, offerMade.Offer.Offerer), TimeSpan.FromMilliseconds(500));

            Assert.IsNotNull(offer);
        }


        [Then(@"I expect an offer on the ExchangeContractActor called ""(.*)"" to be")]
        public void ThenIExpectAnOfferOnTheExchangeContractActorCalledToBe(string contractName, Table table)
        {
            var exchangeContractActor = _state.ExchangeContractActors[contractName];
            var resourceStack = ConstructResourceStack(table);
            var liabilityResourceStack = ConstructLiabilityResourceStack(table);
            var sender = _state.Traders[table.GetField("SenderName")];
            /* Offers on a contract in future may be multiple.
             var queryTask = exchangeContractActor.Ask<ImmutableArray<ExchangeContract.Offer>>(new ExchangeContract.QueryOffers(),
                TimeSpan.FromMilliseconds(50));

             queryTask.Wait();
             */
            var queryTask = exchangeContractActor.Ask<ExchangeContract.Offer>(new ExchangeContract.QueryOffers(),
                TimeSpan.FromMilliseconds(50));

            queryTask.Wait();
            var item = queryTask.Result;
            Assert.AreEqual(resourceStack.Resource, item.ResourceStack.Resource);
            Assert.AreEqual(resourceStack.Quantity, item.ResourceStack.Quantity);
            Assert.AreEqual(liabilityResourceStack.Resource, item.LiabilityStack.Resource);
            Assert.AreEqual(liabilityResourceStack.Quantity, item.LiabilityStack.Quantity);
        }


        [Given(@"I post to the ExchangeContract ""(.*)"" the following invitation using a TestProbe")]
        public void GivenIPostToTheExchangeContractTheFollowingInvitationUsingATestProbe(string exchangeContractName,
            Table table)
        {
            var exchangeContractActor = _state.ExchangeContractActors[exchangeContractName];
            var exchangeType = (OfferType)Enum.Parse(typeof(OfferType), table.GetField("ExchangeType"));

            var resourceName = table.GetField("SellResourceName");
            var sellResource = _state.GetResourceFromName(resourceName);

            var sellResourceQuantity = int.Parse(table.GetField("SellResourceQuantity"));
            var sellResourceTimePeriod =
                (TimePeriodType)Enum.Parse(typeof(TimePeriodType), table.GetField("SellResourceTimePeriod"));
            var sellResourceTimePeriodQuantity = int.Parse(table.GetField("SellResourceTimePeriodQuantity"));

            var suggestedOfferResourceName = table.GetField("SuggestedOfferResourceName");
            var suggestedOfferResource = _state.GetResourceFromName(suggestedOfferResourceName);
            var suggestedQuantity = int.Parse(table.GetField("SuggestedOfferResourceQuantity"));

            var liabilityResourceName = table.GetField("LiabilityResourceName");
            var liabilityResource = _state.GetResourceFromName(liabilityResourceName);
            var liabilityQuantity = int.Parse(table.GetField("LiabilityResourceQuantity"));

            var owner = table.GetField("ContractOwner");
            var contractOwner = _state.TestProbes[owner];

            var postInvitationMessage = new ExchangeContract.PostInvitationMessage(
                exchangeType,
                sellResource,
                sellResourceQuantity,
                sellResourceTimePeriod,
                sellResourceTimePeriodQuantity,
                suggestedOfferResource,
                suggestedQuantity,
                liabilityResource,
                liabilityQuantity);

            exchangeContractActor.Tell(postInvitationMessage, contractOwner);
        }

        [Given(@"the TestProbe called ""(.*)"" makes the following offer on the ExchangeContractActor called ""(.*)""")]
        public void GivenTheTestProbeCalledMakesTheFollowingOfferOnTheExchangeContractActorCalled(string testProbeName,
            string exchangeContractName, Table table)
        {
            var testProbe = _state.TestProbes[testProbeName];
            var exchangeContract = _state.ExchangeContractActors[exchangeContractName];

            var offer = ConstructResourceStack(table);
            var liabilityResourceStack = ConstructLiabilityResourceStack(table);

            exchangeContract.Tell(new ExchangeContract.PostOffer(offer, liabilityResourceStack), testProbe);
        }

        [When(
            @"the TestProbe called ""(.*)"" rejects the offer on the ExchangeContractActor called ""(.*)"" and makes the following suggested offer"
            )]
        public void
            WhenTheTestProbeCalledRejectsTheOfferOnTheExchangeContractActorCalledAndMakesTheFollowingSuggestedOffer(
            string rejectorName, string exchangeContractName, Table table)
        {
            var offer = ConstructResourceStack(table);
            var liabilityResourceStack = ConstructLiabilityResourceStack(table);

            var seller = _state.TestProbes[rejectorName];
            var exchangeContract = _state.ExchangeContractActors[exchangeContractName];

            exchangeContract.Tell(new ExchangeContract.PostRejectOffer(offer, liabilityResourceStack), seller);
        }

        [Then(@"I expect that the TestProbe ""(.*)"" will of recieved the following suggested offer")]
        public void ThenIExpectThatTheTestProbeWillOfRecievedTheFollowingSuggestedOffer(string buyertestProbe,
            Table table)
        {
            var offer = ConstructResourceStack(table);
            var tp = _state.TestProbes[buyertestProbe];

            Assert.IsTrue(tp.HasMessages);

            // var msg = tp.TestActor.Cel;

            tp.ExpectMsg<ExchangeContract.PostRejectOffer>(
                message => offer.Resource.Equals(message.Offer.Resource) && offer.Quantity == message.Offer.Quantity,
                TimeSpan.FromMilliseconds(50));
        }

        [Then(@"I expect the TestProbe ""(.*)"" to of recieved the message Offer Rejected Notification")]
        public void ThenIExpectTheTestProbeToOfRecievedTheMessageOfferRejected(string supervisorName)
        {
            var sup = _state.TestProbes[supervisorName];
            sup.FishForMessage<ExchangeContract.OfferRejectedNotification>((msg) => true, TimeSpan.FromMilliseconds(50));
        }

        [When(@"the TestProbe called ""(.*)"" rejects the offer on the ExchangeContractActor called ""(.*)""")]
        public void WhenTheTestProbeCalledRejectsTheOfferOnTheExchangeContractActorCalled(string sellerName, string exchangeContractActorName)
        {
            var seller = _state.TestProbes[sellerName];
            var contract = _state.ExchangeContractActors[exchangeContractActorName];

            contract.Tell(new ExchangeContract.PostRejectOffer(null, null), seller);
        }

        [Then(@"I expect the TestProbe ""(.*)"" to recieve the following Liability Message")]
        public void ThenIExpectTheTestProbeToRecieveTheFollowingLiabilityMessage(string sellerName, Table table)
        {
            var seller = _state.TestProbes[sellerName];
            var liability = seller.FishForMessage<ExchangeContract.LiabilityReturnedMessage>((msg) => true, TimeSpan.FromMilliseconds(50));
            var resourceName = table.GetField("Resource");
            var resource = _state.GetResourceFromName(resourceName);
            var quantity = int.Parse(table.GetField("Quantity"));

            Assert.AreEqual(resource, liability.LiabilityStack.Resource);
            Assert.AreEqual(quantity, liability.LiabilityStack.Quantity);
        }

        [Then(@"I expect that the TestProbe ""(.*)"" will of recieved an empty Offer Rejected Message")]
        public void ThenIExpectThatTheTestProbeWillOfRecievedAnEmptyOfferRejectedMessage(string testProbe)
        {
            var tp = _state.TestProbes[testProbe];
            var msg = tp.ExpectMsg<ExchangeContract.PostRejectOffer>(TimeSpan.FromMilliseconds(50));

            Assert.IsNull(msg.Offer);
            Assert.IsNull(msg.Liability);

        }


    }
}
