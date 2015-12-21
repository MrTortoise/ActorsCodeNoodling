using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using Akka.Util.Internal;
using NUnit.Framework;
using Serilog;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Entities.Model.ResourceManagerFeature
{
    [Binding]
    public class ResourceManagerSteps
    {
        public const string ResourceManagerName = "resourceManager";
        private const string ResourceManagerTestProbeName = "resourceManagerTestProbe";
        private readonly ScenarioContextState _state;

       public ResourceManagerSteps(ScenarioContextState state)
       {
          _state = state;
       }

        [Given(@"I create a Resource Manager")]
        public void CreateResourceMAnager()
        {
            GivenICreateAResourceManager(_state);
        }

        public static void GivenICreateAResourceManager(ScenarioContextState scenarioContextState)
        {
            var resourceManagerTestProbe = new TestProbe(scenarioContextState.TestKit.Sys, new NUnitAssertions(),
                ResourceManagerTestProbeName);
            var resourceManager = scenarioContextState.TestKit.ActorOfAsTestActorRef<ResourceManager>(resourceManagerTestProbe);

            scenarioContextState.TestProbes.Add(ResourceManagerTestProbeName, resourceManagerTestProbe);

            scenarioContextState.Actors.Add(ResourceManagerTestProbeName, resourceManager);
            scenarioContextState.ResourceManager = resourceManager;
            scenarioContextState.Actors.Add(ResourceManagerName, resourceManager);
        }


        [Given(@"I add the following resources to the Resource Manager")]
        [When(@"I add the following resources to the Resource Manager")]
        public void WhenIPressAddTheFollowingResourcesToTheResourceManager(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resourceManagerActorRef = _state.ResourceManager;
            resources.ForEach(r => resourceManagerActorRef.Tell(new ResourceManager.PostResourceMessage(r)));
            Thread.Sleep(10);
        }

        public static Resource[] CreateResourcesFromTable(Table table)
        {
            return table.Rows.Select(r => new Resource(r["name"])).ToArray();
        }

        [Then(@"the I expect the Resource  Manager to contain the following resources")]
        public void ThenTheIExpectTheResourceManagerToContainTheFollowingResources(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resManagerActor = _state.ResourceManager;
            List<Task<ResourceManager.GetResourceResult>> tasks = new List<Task<ResourceManager.GetResourceResult>>();

            resources.ForEach(r =>
            {
                var t = resManagerActor.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(r.Name), TimeSpan.FromSeconds(2));
                tasks.Add(t);
            });

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks.ToArray());

            var results = tasks.Select(i => i.Result.Values[0].Name).ToList();
            foreach (string resourceName in resources.Select(r => r.Name))
            {
                Assert.Contains(resourceName, results);
            }
        }
    }
}
