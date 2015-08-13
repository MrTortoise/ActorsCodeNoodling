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
        [Given(@"I add the following resources to the Resource Manager")]
        [When(@"I add the following resources to the Resource Manager")]
        public void WhenIPressAddTheFollowingResourcesToTheResourceManager(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resourceManagerActorRef = ScenarioContext.Current.GetResourceManagerActorRef();
            resources.ForEach(r => resourceManagerActorRef.Tell(new ResourceManager.PostResourceMessage(r)));
        }

        public static Resource[] CreateResourcesFromTable(Table table)
        {
            return table.Rows.Select(r => new Resource(r["name"])).ToArray();
        }

        //public static IActorRef GetResourceManagerActorRef()
        //{
        //    Assert.IsNotNull(ScenarioContext.Current);
        //    ScenarioContext.Current.Keys.ForEach(Console.WriteLine);
        //    return (IActorRef)ScenarioContext.Current[Constants.ResourceManager];
        //}

        [Then(@"the I expect the Resource  Manager to contain the following resources")]
        public void ThenTheIExpectTheResourceManagerToContainTheFollowingResources(Table table)
        {
            var resources = CreateResourcesFromTable(table);
            var resManagerActor = ScenarioContext.Current.GetResourceManagerActorRef();
            List<Task<IResource>> tasks = new List<Task<IResource>>();

            resources.ForEach(r =>
            {
                var t = resManagerActor.Ask<IResource>(new ResourceManager.GetResource(r.Name),TimeSpan.FromSeconds(2));
                tasks.Add(t);
            });

            // ReSharper disable once CoVariantArrayConversion
            Task.WaitAll(tasks.ToArray());

            var results = tasks.Select(i => i.Result.Name).ToList();
            foreach (string resourceName in resources.Select(r=>r.Name))
            {
                Assert.Contains(resourceName, results);
            }
        }
    }
}
