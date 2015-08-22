using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

       [Given(@"I create an ExchangeContractActor called ""(.*)""")]
       [When(@"I create an ExchangeContractActor called ""(.*)""")]
       public void GivenICreateAnExchangeContractActorCalled(string name)
      {
         var exchangeContractActor = _state.TestKit.ActorOfAsTestActorRef<ExchangeContract>(name);
          _state.ExchangeContractActors.Add(name, exchangeContractActor);
      }

       [Given(@"I have configured the DateTime provider to return ""(.*)""")]
        public void GivenIHaveConfiguredTheDateTimeProviderToReturn(string dateTimeString)
       {
          var dateTime = DateTime.Parse(dateTimeString);
          DateTimeProvider.SetProvider(new FixedDateTime(dateTime));
       }

      [Given(@"I post to the ExchangeContract ""(.*)"" the following invitation")]
      [When(@"I post to the ExchangeContract ""(.*)"" the following invitation")]
       public void WhenIPostToTheExchangeContractTheFollowingInvitation(string exchangeContractName, Table table)
      {
         var exchangeContractActor = _state.ExchangeContractActors[exchangeContractName];
          var exchangeType = (OfferType)Enum.Parse(typeof (OfferType), table.GetField("ExchangeType"));

          var resourceName = table.GetField("SellResourceName");
          var sellResource = _state.GetResourceFromName(resourceName);

          var sellResourceQuantity = int.Parse(table.GetField("SellResourceQuantity"));
          var sellResourceTimePeriod = (TimePeriodType)Enum.Parse(typeof (TimePeriodType), table.GetField("SellResourceTimePeriod"));
          var sellResourceTimePeriodQuantity = int.Parse(table.GetField("SellResourceTimePeriodQuantity"));

          var suggestedOfferResourceName = table.GetField("SuggestedOfferResourceName");
          var suggestedOfferResource = _state.GetResourceFromName(suggestedOfferResourceName);
          var suggestedQuantity = int.Parse(table.GetField("SuggestedOfferResourceQuantity"));

         var liabilityResourceName = table.GetField("LiabilityResourceName");
         var liabilityResource = _state.GetResourceFromName(liabilityResourceName);
         var liabilityQuantity = int.Parse(table.GetField("LiabilityResourceQuantity"));

         var contractOwner = _state.Traders[table.GetField("ContractOwner")];

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
        public void ThenIExpectTheStateOfTheExchangeContractActorToBe(string exchangeContractActorName, string exchangeContractActorState)
       {
          var exchangeActorRef = _state.ExchangeContractActors[exchangeContractActorName];
          var stateTask = exchangeActorRef.Ask<ExchangeContract.State>(new ExchangeContract.QueryStateMessage(), TimeSpan.FromMilliseconds(50));
          stateTask.Wait();
          var expectedState = (ExchangeContract.State) Enum.Parse(typeof (ExchangeContract.State), exchangeContractActorState);

          Assert.AreEqual(expectedState, stateTask.Result);
       }

       [Then(@"I expect the creator of ExchangeContractActor ""(.*)"" to be ""(.*)""")]
       public void ThenIExpectTheCreatorOfExchangeContractActorToBe(string exchangeContractActorName, string creator)
       {
          var exchangeActorRef = _state.ExchangeContractActors[exchangeContractActorName];
          var creatorActorRef = _state.Traders[creator];

          var ownerTask = exchangeActorRef.Ask<IActorRef>(new ExchangeContract.QueryOwner(), TimeSpan.FromMilliseconds(50));
          ownerTask.Wait();
          var owner = ownerTask.Result;

           Assert.AreEqual(creatorActorRef,owner);   
       }

       [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following for offer")]
       public void ThenIExpectTheExchangeContractActorToHaveTheFollowingForOffer(string exchangeContract, Table table)
       {
          var exchangeActorRef = _state.ExchangeContractActors[exchangeContract];
          var queryTask = exchangeActorRef.Ask<InvitationToTreat>(new ExchangeContract.QueryInvitationToTreat(), TimeSpan.FromMilliseconds(50));

          queryTask.Wait();
          var invitation = queryTask.Result;

          var exchangeType = Enum.Parse(typeof (OfferType), table.GetField("ExchangeType"));
          var resource = _state.GetResourceFromName(table.GetField("Resource"));
          var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("Quantity")));
          var dateTime = DateTime.Parse(table.GetField("CompletionTime"));

          resource = _state.GetResourceFromName(table.GetField("SuggestedOfferResourceName"));
          var suggestedResource = new ResourceStack(resource, int.Parse(table.GetField("SuggestedOfferResourceQuantity")));

          resource = _state.GetResourceFromName(table.GetField("LiabilityResourceName"));
          var liabilityResource = new ResourceStack(resource, int.Parse(table.GetField("LiabilityResourceQuantity")));

          Assert.AreEqual(exchangeType, invitation.ExchangeType);
          Assert.AreEqual(resourceStack, invitation.InvitationStack);
          Assert.AreEqual(dateTime, invitation.InvitationDeadline);
          Assert.AreEqual(suggestedResource, invitation.SuggestedOffer);
          Assert.AreEqual(liabilityResource, invitation.LiabilityStack);
       }

      [When(@"the Trader called ""(.*)"" makes the following offer on the ExchangeContractActor called ""(.*)""")]
      public void WhenTheTraderCalledMakesTheFollowingOfferOnTheExchangeContractActorCalled(string buyerName, string exchangeContractName, Table table)
      {
         var buyerActorRef = _state.Traders[buyerName];
         var exchangeActorRef = _state.ExchangeContractActors[exchangeContractName];

         var resource = _state.GetResourceFromName(table.GetField("Resource"));
         var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("Quantity")));

         exchangeActorRef.Tell(new ExchangeContract.PostOffer(resourceStack), buyerActorRef); 
      }

      //[Then(@"I expect that the Trader ""(.*)"" will of been notified of an offer being made")]
      //public void ThenIExpectThatTheTraderWillOfBeenNotifiedOfAnOfferBeingMade(string sellerName)
      //{

      //}    

      [Then(@"I expect an offer on the ExchangeContractActor called ""(.*)"" to be")]
      public void ThenIExpectAnOfferOnTheExchangeContractActorCalledToBe(string contractName, Table table)
      {
         var exchangeContractActor = _state.ExchangeContractActors[contractName];
         var resource = _state.GetResourceFromName(table.GetField("Resource"));
         int quantity = int.Parse(table.GetField("Quantity"));
         var sender = _state.Traders[table.GetField("SenderName")];

         _state.TestKit.ExpectMsg<ExchangeContract.OfferMade>((offerMade) => resource.Equals(offerMade.Offer.ResourceStack.Resource)
                                                                             && quantity == offerMade.Offer.ResourceStack.Quantity
                                                                             && ReferenceEquals(sender,offerMade.Offer.Offerer), TimeSpan.FromMilliseconds(500));


      }

      [Given(@"the Trader called ""(.*)"" makes the following offer on the ExchangeContractActor called ""(.*)""")]
      public void GivenTheTraderCalledMakesTheFollowingOfferOnTheExchangeContractActorCalled(string p0, string p1, Table table)
      {
         ScenarioContext.Current.Pending();
      }

      [When(@"the Trader called ""(.*)"" rejects the offer on the ExchangeContractActor called ""(.*)"" and makes the following suggested offer")]
      public void WhenTheTraderCalledRejectsTheOfferOnTheExchangeContractActorCalledAndMakesTheFollowingSuggestedOffer(string p0, string p1, Table table)
      {
         ScenarioContext.Current.Pending();
      }

      [Then(@"I expect that the Trader ""(.*)"" will of been notified of a suggested offer being made")]
      public void ThenIExpectThatTheTraderWillOfBeenNotifiedOfASuggestedOfferBeingMade(string p0)
      {
         ScenarioContext.Current.Pending();
      }

      [Then(@"I expect the suggested offer on the ExchangeContractActor called ""(.*)"" to be")]
      public void ThenIExpectTheSuggestedOfferOnTheExchangeContractActorCalledToBe(string p0, Table table)
      {
         ScenarioContext.Current.Pending();
      }

   }
}
