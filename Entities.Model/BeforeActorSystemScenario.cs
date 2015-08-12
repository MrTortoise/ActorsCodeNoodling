using System.Diagnostics;
using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class BeforeScenarioTags
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
            var actorSystem = GetActorSystem();
            actorSystem.Shutdown();
        }

        private static ActorSystem GetActorSystem()
        {
            return (ActorSystem) ScenarioContext.Current[Constants.TestActorSystemName];
        }

        [BeforeScenario("resourceManager")]
        public static void BeforeResourceManagerScenario()
        {
            var actorSystem = GetActorSystem();
            var resourceManagerActorRef = actorSystem.ActorOf<ResourceManager>("resourceManager");
            ScenarioContext.Current.Add(Constants.ResourceManager, resourceManagerActorRef);
        }
    }
}
