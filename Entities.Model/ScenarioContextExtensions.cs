﻿using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.Factories;
using Entities.Model.CenterOfMassActors;
using Entities.Model.FactoryTests;
using Entities.Model.Heartbeats;
using Entities.Model.Inventory;
using Entities.Model.ResourceManagerFeature;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    public static class ScenarioContextExtensions
    {
        public static ImmutableDictionary<IResource, double> GetResourceComposition(this ScenarioContextState scenarioContextState, Table table)
        {
            var resources =
                scenarioContextState.ResourceManager.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(null));

            resources.Wait();

            var resourceCompositionBuilder = ImmutableDictionary.CreateBuilder<IResource, double>();

            foreach (var tableRow in table.Rows)
            {
                var resourceName = tableRow["ResourceName"];
                double val = double.Parse(tableRow["Value"]);
                var resource = resources.Result.Values.Single(i => i.Name == resourceName);
                resourceCompositionBuilder.Add(resource, val);
            }
            return resourceCompositionBuilder.ToImmutable();
        }

        public static InventoryType GetInventoryType(this ScenarioContextState scenarioContextState, string inventoryTypeName)
        {
            var inventoryTypesQueryTask = scenarioContextState.InventoryActorCoordinator.Ask<InventoryTypeCoordinator.InventoryTypesResult>(new InventoryTypeCoordinator.InventoryTypesQuery());
            inventoryTypesQueryTask.Wait();

            var result = inventoryTypesQueryTask.Result.InventoryTypes.SingleOrDefault(i => i.Name == inventoryTypeName);
            return result;
        }

        public static void SetupActorCoordinators(this ScenarioContextState scenarioContextState)
        {
            ResourceManagerSteps.GivenICreateAResourceManager(scenarioContextState);
            FactoryUpdateSteps.GivenIHaveCreatedAFactoryCoordinatorActor(scenarioContextState);
            InventorySteps.CreateInventoryActorCoordinator(scenarioContextState);
            CenterOfMassSteps.CreateCenterOfMassManagerActor(scenarioContextState);
            HeartbeatSteps.CreateHeartbeatActor(scenarioContextState);
        }

        public static FactoryCoordinatorActor.FactoryQueryResult GetFactories(this ScenarioContextState scenarioContextState)
        {
            var coordinatorFactories = scenarioContextState.FactoryCoordinator.Actor.Ask<FactoryCoordinatorActor.FactoryQueryResult>(new FactoryCoordinatorActor.QueryFactories());
            coordinatorFactories.Wait();
            var factoryQueryResult = coordinatorFactories.Result;
            return factoryQueryResult;
        }

    }
}
