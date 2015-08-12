using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Entities.Model.ResourceManagerFeature
{
    [Binding]
    public class ResourceManagerSteps
    {
        [When(@"I press add the following resources to the Resource Manager")]
        public void WhenIPressAddTheFollowingResourcesToTheResourceManager(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resourceManagerActorRef = GetResourceManagerActorRef();
            resources.ForEach(r => resourceManagerActorRef.Tell(new ResourceManager.PostResourceMessage(r)));
        }

        public static Resource[] CreateResourcesFromTable(Table table)
        {
            return table.Rows.Select(r => new Resource(r["name"])).ToArray();
        }

        public static IActorRef GetResourceManagerActorRef()
        {
            Assert.IsNotNull(ScenarioContext.Current);
            ScenarioContext.Current.Keys.ForEach(Console.WriteLine);
            return (IActorRef)ScenarioContext.Current[Constants.ResourceManager];
        }

        [Then(@"the I expect the Resource  Manager to contain the following resources")]
        public void ThenTheIExpectTheResourceManagerToContainTheFollowingResources(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resManagerActor = GetResourceManagerActorRef();
            List<Task<Resource>> tasks = new List<Task<Resource>>();

            resources.ForEach(r =>
            {
                var t = resManagerActor.Ask<Resource>(new ResourceManager.GetResource(r.Name),TimeSpan.FromSeconds(2));
                tasks.Add(t);
            });

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks.ToArray());

            tasks.ForEach(t=>Assert.IsTrue(t.IsCompleted,t.Exception.Flatten().ToString()));

            var results = tasks.Select(i => i.Result.Name).ToList();
            foreach (string resourceName in resources.Select(r=>r.Name))
            {
                Assert.Contains(resourceName, results);
            }
        }
    }
}
