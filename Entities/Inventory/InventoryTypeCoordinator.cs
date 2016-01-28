using System.Collections.Immutable;
using Akka.Actor;

namespace Entities.Inventory
{
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