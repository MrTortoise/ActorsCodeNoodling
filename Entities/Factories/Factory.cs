using Akka.Actor;
using Entities.LocationActors;

namespace Entities.Factories
{
    public class Factory : ReceiveActor
    {
        private readonly FactoryState _factoryState;

        public Factory(string name, FactoryType factoryType, CelestialBody body, InventoryType inventoryType)
        {
            var inventory = new Inventory(inventoryType);
            _factoryState = new FactoryState(body, name, factoryType,inventory);

            Receive<QueryState>(msg =>
            {
                Sender.Tell(_factoryState);
            });
        }

        public static Props CreateProps(string name, FactoryType factoryType, CelestialBody body, InventoryType inventoryType)
        {
            return Props.Create(() => new Factory(name, factoryType, body, inventoryType));
        }

        public class FactoryState
        {
            public CelestialBody Body { get; private set; }
            public string Name { get; private set; }
            public FactoryType FactoryType { get; private set; }
            public Inventory Inventory { get; private set; }

            public FactoryState(CelestialBody body, string name, FactoryType factoryType, Inventory inventory)
            {
                Body = body;
                Name = name;
                FactoryType = factoryType;
                Inventory = inventory;
            }
        }

        public class QueryState
        {
        }
    }
}