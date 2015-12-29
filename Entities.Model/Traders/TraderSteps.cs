using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Entities.Model.Traders
{
    [Binding]
    public class TraderSteps
    {
       private readonly ScenarioContextState _state;

       public TraderSteps(ScenarioContextState state)
       {
          _state = state;
       }

       [Given(@"I have created a Trader called ""(.*)""")]
        public void GivenIHaveCreatedATraderCalled(string name)
       {
          var traderActor = _state.TestKit.ActorOfAsTestActorRef<Trader>(Props.Create(() => new Trader(name)));
          _state.Traders.Add(name, traderActor);
       }
        
        /// <summary>
        /// Takes a trader and set of resources with quantities. Looks up the resources and posts them to the trader
        /// </summary>
        /// <param name="name">The name of the trader</param>
        /// <param name="table">A resource name, quantity set of fields</param>
        [When(@"I post the folowing resources to the Trader ""(.*)""")]
        public void WhenIPostTheFolowingResourcesToTheTrader(string name, Table table)
        {
           var resourceManager = _state.Actors[ResourceManager.Name];
            List<Task<ResourceManager.GetResourceResult>> resourceTasks = new List<Task<ResourceManager.GetResourceResult>>();

            // get the resources
            table.Rows.ForEach(r =>
            {
                var t = resourceManager.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(r["name"]));
                resourceTasks.Add(t);
            });

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(resourceTasks.ToArray());
            var resources = resourceTasks.Select(rt => rt.Result);

            //create the post messages
            var messages = table.Rows.Select(r =>
            {
                var resource = resources.Single(res => res.Values[0].Name.Equals(r["name"]));
                return new Trader.PostResourceMessage(new ResourceStack(resource.Values[0], Convert.ToInt32(r["quantity"])));
            });

            // post them
            var trader = _state.Traders[name];
            messages.ForEach(m => trader.Tell(m));
        }

       [When(@"I ask What resources Trader ""(.*)"" has storing them in the context as ""(.*)""")]
        public void WhenIAskWhatResourcesTraderHasStoringThemInTheContextAs(string name, string resourceString)
        {
            var trader = _state.Traders[name];
            var query = trader.Ask<Trader.QueryResourcesResultMessage>(new Trader.QueryResourcesMessage(),TimeSpan.FromMilliseconds(40));
            Task.WaitAll(query);
            var resources = query.Result;
            ScenarioContext.Current.Add(resourceString, resources);
        }
        
        [Then(@"I expect the following resources in context ""(.*)""")]
        public void ThenIExpectTheFollowingResourcesInContext(string resourceString, Table table)
        {
            var resources = (Trader.QueryResourcesResultMessage) ScenarioContext.Current[resourceString];
            var resourceRows = table.CreateSet<ResourceRow>();

            resourceRows.ForEach(rr =>
            {
                Assert.IsTrue(resources.ResourceStacks.Select(r => r.Resource.Name).Contains(rr.Name));
                var resource = resources.ResourceStacks.Single(r => r.Resource.Name.Equals(rr.Name));

                Assert.AreEqual(rr.Name, resource.Resource.Name);
                Assert.AreEqual(rr.Quantity, resource.Quantity);
            });
        }

        public class ResourceRow
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
