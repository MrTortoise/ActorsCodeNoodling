using System;
using System.Linq;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Entities.Model.ContractActors
{
    [Binding]
    public class ExchangeContractFeatureSteps
    {
       [Given(@"I create an ExchangeContractActor called ""(.*)""")]
       [When(@"I create an ExchangeContractActor called ""(.*)""")]
       public void GivenICreateAnExchangeContractActorCalled(string name)
       {
         var system = ScenarioContext.Current.GetActorSystem();
          var exchangeContractActor = system.ActorOf<ExchangeContract>(name);
          ScenarioContext.Current.Add("ExchangeContract_" + name, exchangeContractActor);
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
          var exchangeContractActor = SpecflowHelpers.GetExchangeContractActor(exchangeContractName);
          var exchangeType = (OfferType)Enum.Parse(typeof (OfferType), table.GetField("ExchangeType"));

          var resourceName = table.GetField("SellResourceName");
          var sellResource = SpecflowHelpers.GetResourceFromName(resourceName);

          var sellResourceQuantity = int.Parse(table.GetField("SellResourceQuantity"));
          var sellResourceTimePeriod = (TimePeriodType)Enum.Parse(typeof (TimePeriodType), table.GetField("SellResourceTimePeriod"));
          var sellResourceTimePeriodQuantity = int.Parse(table.GetField("SellResourceTimePeriodQuantity"));

          var suggestedOfferResourceName = table.GetField("SuggestedOfferResourceName");
          var suggestedOfferResource = SpecflowHelpers.GetResourceFromName(suggestedOfferResourceName);
          var suggestedQuantity = int.Parse(table.GetField("SuggestedOfferResourceQuantity"));

         var liabilityResourceName = table.GetField("LiabilityResourceName");
         var liabilityResource = SpecflowHelpers.GetResourceFromName(liabilityResourceName);
         var liabilityQuantity = int.Parse(table.GetField("LiabilityResourceQuantity"));

         var contractOwner = SpecflowHelpers.GetTraderActorFromName(table.GetField("ContractOwner"));

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
          var exchangeActorRef = SpecflowHelpers.GetExchangeContractActor(exchangeContractActorName);
          var stateTask = exchangeActorRef.Ask<ExchangeContract.State>(new ExchangeContract.QueryStateMessage(), TimeSpan.FromMilliseconds(50));
          stateTask.Wait();
          var expectedState = (ExchangeContract.State) Enum.Parse(typeof (ExchangeContract.State), exchangeContractActorState);

          Assert.AreEqual(expectedState, stateTask.Result);
       }

       [Then(@"I expect the creator of ExchangeContractActor ""(.*)"" to be ""(.*)""")]
       public void ThenIExpectTheCreatorOfExchangeContractActorToBe(string exchangeContractActorName, string creator)
       {
          var exchangeActorRef = SpecflowHelpers.GetExchangeContractActor(exchangeContractActorName);
          var creatorActorRef = SpecflowHelpers.GetTraderActorFromName(creator);

          var ownerTask = exchangeActorRef.Ask<IActorRef>(new ExchangeContract.QueryOwner(), TimeSpan.FromMilliseconds(50));
          ownerTask.Wait();
          var owner = ownerTask.Result;

           Assert.AreEqual(creatorActorRef,owner);   
       }

       [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following for offer")]
       public void ThenIExpectTheExchangeContractActorToHaveTheFollowingForOffer(string exchangeContract, Table table)
       {
          var exchangeActorRef = SpecflowHelpers.GetExchangeContractActor(exchangeContract);
          var queryTask = exchangeActorRef.Ask<InvitationToTreat>(new ExchangeContract.QueryInvitationToTreat(), TimeSpan.FromMilliseconds(50));

          queryTask.Wait();
          var invitation = queryTask.Result;

          var exchangeType = Enum.Parse(typeof (OfferType), table.GetField("ExchangeType"));
          var resource = SpecflowHelpers.GetResourceFromName(table.GetField("Resource"));
          var resourceStack = new ResourceStack(resource, int.Parse(table.GetField("Quantity")));
          var dateTime = DateTime.Parse(table.GetField("CompletionTime"));

          resource = SpecflowHelpers.GetResourceFromName(table.GetField("SuggestedOfferResourceName"));
          var suggestedResource = new ResourceStack(resource, int.Parse(table.GetField("SuggestedOfferResourceQuantity")));

          resource = SpecflowHelpers.GetResourceFromName(table.GetField("LiabilityResourceName"));
          var liabilityResource = new ResourceStack(resource, int.Parse(table.GetField("LiabilityResourceQuantity")));

          Assert.AreEqual(exchangeType, invitation.ExchangeType);
          Assert.AreEqual(resourceStack, invitation.InvitationStack);
          Assert.AreEqual(dateTime, invitation.InvitationDeadline);
          Assert.AreEqual(suggestedResource, invitation.SuggestedOffer);
          Assert.AreEqual(liabilityResource, invitation.LiabilityStack);
       }  
    }
}
