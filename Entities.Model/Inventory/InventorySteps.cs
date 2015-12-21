using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.Util.Internal;
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

        public static void CreateInventoryActorCoordinator(ScenarioContextState state)
        {
            var inventoryActorCoordinator = state.TestKit.Sys.ActorOf(InventoryTypeCoordinator.CreateProps(), InventoryTypeCoordinator.Name);
            state.Actors.Add(InventoryTypeCoordinator.Name, inventoryActorCoordinator);
            state.InventoryActorCoordinator = inventoryActorCoordinator;
            Thread.Sleep(10);
        }

        [Given(@"I have created the following inventory types")]
        [When(@"I have created the following inventory types")]
        public void WhenIHaveCreatedTheFollowingInventoryTypes(Table table)
        {
            var inventoryTypes = table.GetInventoryTypes();
            foreach (var inventoryType in inventoryTypes)
            {
                _state.InventoryActorCoordinator.Tell(new InventoryTypeCoordinator.AddInventoryType(inventoryType));
                Thread.Sleep(10);
            }
        }

        [Then(@"I expect the following inventory types to exist")]
        public void ThenIExpectTheFollowingInventoryTypesToExist(Table table)
        {
            var inventoryTypes = table.GetInventoryTypes();
            var inventoryTypesQueryTask = _state.InventoryActorCoordinator.Ask<InventoryTypeCoordinator.InventoryTypesResult>(new InventoryTypeCoordinator.InventoryTypesQuery());

            inventoryTypesQueryTask.Wait();
            Assert.IsTrue(inventoryTypes.Any(i => inventoryTypesQueryTask.Result.InventoryTypes.SingleOrDefault(d => d.Name == i.Name && d.Capacity == i.Capacity && d.Size == i.Size) != null));
        }
    }

    public class InventoryTypeCoordinator : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new InventoryTypeCoordinator());
        }

        public static string Name = "InventoryTypeCoordinator";
        private ImmutableHashSet<InventoryType> _inventoryTypes = ImmutableHashSet<InventoryType>.Empty;

        public InventoryTypeCoordinator()
        {
            Receive<AddInventoryType>(msg =>
            {
                _inventoryTypes = _inventoryTypes.Add(msg.InventoryType);
            });

            Receive<InventoryTypesQuery>(msg =>
            {
                Sender.Tell(new InventoryTypesResult(_inventoryTypes));
            });
        }

        public class AddInventoryType
        {
            public InventoryType InventoryType { get; set; }

            public AddInventoryType(InventoryType inventoryType)
            {
                InventoryType = inventoryType;
            }
        }

        public class InventoryTypesResult
        {
            public ImmutableHashSet<InventoryType> InventoryTypes { get; set; }


            public InventoryTypesResult(ImmutableHashSet<InventoryType> inventoryTypes)
            {
                InventoryTypes = inventoryTypes;
            }
        }

        public class InventoryTypesQuery
        {
        }
    }
}
