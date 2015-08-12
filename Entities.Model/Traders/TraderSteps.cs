using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Entities.Model.Traders
{
    [Binding]
    public class TraderSteps
    {
        private readonly Dictionary<string, IActorRef> _traders;

        public TraderSteps(Dictionary<string, IActorRef> traders, ResourceManager resourceManager)
        {
            _traders = traders;
        }

        [Given(@"I have created a Trader called ""(.*)""")]
        public void GivenIHaveCreatedATraderCalled(string name)
        {
            var system = (ActorSystem) ScenarioContext.Current[Constants.TestActorSystemName];

            var traderActor = system.ActorOf(Props.Create<Trader>(args: new object[] {name}));
            
            _traders.Add(name, traderActor);
        }
        
        [When(@"I post the folowing resources to the Trader ""(.*)""")]
        public void WhenIPostTheFolowingResourcesToTheTrader(string name, Table table)
        {
            var messages = table.Rows.Select(r =>
            {
                var resource = new Resource(r["Name"]);
                return new Trader.PostResourceMessage(resource, Convert.ToInt32(r["Quantity"]));
            });

            var trader = _traders[name];
            messages.ForEach(m => trader.Tell(m));
        }
        
        [When(@"I ask What resources Trader ""(.*)"" has storing them in the context as ""(.*)""")]
        public void WhenIAskWhatResourcesTraderHasStoringThemInTheContextAs(string name, string resourceString)
        {
            var trader = _traders[name];
            var query = trader.Ask<Trader.QueryResourcesResultMessage>(new Trader.QueryResourcesMessage());
            Task.WaitAll(query);
            var resources = query.Result;
            ScenarioContext.Current.Add(resourceString, resources);
        }
        
        [Then(@"I expect the following resources in context ""(.*)""")]
        public void ThenIExpectTheFollowingResourcesInContext(string resourceString, Table table)
        {
            var resources = (Trader.QueryResourcesResultMessage) ScenarioContext.Current[resourceString];
            var resourceRows = table.CreateSet<ResourceRow>();
            

        }

        public class ResourceRow
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
