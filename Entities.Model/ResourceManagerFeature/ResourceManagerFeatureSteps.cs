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
        public void GivenICreateAResourceManager()
        {
            var resourceManagerTestProbe = new TestProbe(_state.TestKit.Sys, new NUnitAssertions(),
                ResourceManagerTestProbeName);
            var resourceManager = _state.TestKit.ActorOfAsTestActorRef<ResourceManager>(resourceManagerTestProbe);

            _state.TestProbes.Add(ResourceManagerTestProbeName, resourceManagerTestProbe);

            _state.Actors.Add(ResourceManagerTestProbeName, resourceManager);
            _state.ResourceManager = resourceManager;
            _state.Actors.Add(ResourceManagerName, resourceManager);
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
            List<Task<IResource>> tasks = new List<Task<IResource>>();

            resources.ForEach(r =>
            {
                var t = resManagerActor.Ask<IResource>(new ResourceManager.GetResource(r.Name), TimeSpan.FromSeconds(2));
                tasks.Add(t);
            });

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks.ToArray());

            var results = tasks.Select(i => i.Result.Name).ToList();
            foreach (string resourceName in resources.Select(r => r.Name))
            {
                Assert.Contains(resourceName, results);
            }
        }
    }
}
