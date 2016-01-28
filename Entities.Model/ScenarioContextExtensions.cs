using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.Factories;
using Entities.Inventory;
using Entities.Model.CenterOfMassActors;
using Entities.Model.FactoryTests;
using Entities.Model.Heartbeats;
using Entities.Model.Inventory;
using Entities.Model.ResourceManagerFeature;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    public static class ScenarioContextExtensions
    {
        public static ImmutableDictionary<IResource, int> GetResourceComposition(this ScenarioContextState scenarioContextState, Table table)
        {
            var actorRef = scenarioContextState.Actors[ResourceManager.Name];
            Assert.IsNotNull(actorRef,"Resource manager was null - actor system not initialised in proper order?");
            var resources =
                actorRef.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(null));

            resources.Wait();

            var resourceCompositionBuilder = ImmutableDictionary.CreateBuilder<IResource, int>();

            foreach (var tableRow in table.Rows)
            {
                var resourceName = tableRow["ResourceName"];
                int val = int.Parse(tableRow["Value"]);
                var resource = resources.Result.Values.Single(i => i.Name == resourceName);
                resourceCompositionBuilder.Add(resource, val);
            }
            return resourceCompositionBuilder.ToImmutable();
        }

        public static InventoryType GetInventoryType(this ScenarioContextState scenarioContextState, string inventoryTypeName)
        {
            var inventoryTypesQueryTask = RootLevelActors.InventoryTypeCoordinatorActorRef.Ask<InventoryTypeCoordinator.InventoryTypesResult>(new InventoryTypeCoordinator.InventoryTypesQuery());
            inventoryTypesQueryTask.Wait();

            var result = inventoryTypesQueryTask.Result.InventoryTypes.SingleOrDefault(i => i.Name == inventoryTypeName);
            return result;
        }

        public static FactoryCoordinatorActor.FactoryQueryResult GetFactories(this ScenarioContextState scenarioContextState)
        {
            var coordinatorFactories = RootLevelActors.FactoryCoordinatorActorRef.Ask<FactoryCoordinatorActor.FactoryQueryResult>(new FactoryCoordinatorActor.QueryFactories());
            coordinatorFactories.Wait();
            var factoryQueryResult = coordinatorFactories.Result;
            return factoryQueryResult;
        }
    }
}
