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

          var ownerTask = exchangeActorRef.Ask<string>(new ExchangeContract.QueryOwner(), TimeSpan.FromMilliseconds(50));
          ownerTask.Wait();
          var owner = ownerTask.Result;

          var creatorDtoTask = creatorActorRef.Ask<ExchangeContract>(new ExchangeContract.QueryDto(),
             TimeSpan.FromMilliseconds(50));
          creatorDtoTask.Wait();


          ScenarioContext.Current.Pending();
       }

       [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following for offer")]
        public void ThenIExpectTheExchangeContractActorToHaveTheFollowingForOffer(string p0, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following suggested offer of Resource ""(.*)"" and Quantity ""(.*)""")]
        public void ThenIExpectTheExchangeContractActorToHaveTheFollowingSuggestedOfferOfResourceAndQuantity(string p0, string p1, int p2)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I expect the ExchangeContractActor ""(.*)"" to have the following liability of Resource ""(.*)"" and Quantity ""(.*)""")]
        public void ThenIExpectTheExchangeContractActorToHaveTheFollowingLiabilityOfResourceAndQuantity(string p0, string p1, int p2)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
