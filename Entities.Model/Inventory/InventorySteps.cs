using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.Inventory;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.Inventory
{
    [Binding]
    public class InventorySteps
    {
        private readonly ScenarioContextState _state;

        public InventorySteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I have created the following inventory types")]
        [When(@"I have created the following inventory types")]
        public void WhenIHaveCreatedTheFollowingInventoryTypes(Table table)
        {
            var inventoryTypes = table.GetInventoryTypes();
            foreach (var inventoryType in inventoryTypes)
            {
            RootLevelActors.InventoryTypeCoordinatorActorRef.Tell(new InventoryTypeCoordinator.AddInventoryType(inventoryType));
                Thread.Sleep(10);
            }
        }

        [Then(@"I expect the following inventory types to exist")]
        public void ThenIExpectTheFollowingInventoryTypesToExist(Table table)
        {
            var inventoryTypes = table.GetInventoryTypes();
            var inventoryTypesQueryTask = RootLevelActors.InventoryTypeCoordinatorActorRef.Ask<InventoryTypeCoordinator.InventoryTypesResult>(new InventoryTypeCoordinator.InventoryTypesQuery());

            inventoryTypesQueryTask.Wait();
            Assert.IsTrue(inventoryTypes.Any(i => inventoryTypesQueryTask.Result.InventoryTypes.SingleOrDefault(d => d.Name == i.Name && d.Capacity == i.Capacity && d.Size == i.Size) != null));
        }
    }
}
