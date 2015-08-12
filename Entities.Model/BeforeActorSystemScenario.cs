using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    public static class BeforeScenarioTags
    {
        [BeforeScenario("actorSystem")]
        public static void BeforeActorSystemScenario()
        {
            var actorSystem = ActorSystem.Create(Constants.TestActorSystemName);
            ScenarioContext.Current.Add(Constants.TestActorSystemName, actorSystem);
        }

        [AfterScenario("actorSystem")]
        public static void AfterActorSystemScenario()
        {
            var actorSystem = (ActorSystem) ScenarioContext.Current[Constants.TestActorSystemName];
            actorSystem.Shutdown();
        }
    }
}
