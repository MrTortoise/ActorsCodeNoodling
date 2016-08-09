using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Entities.Inventory;
using Entities.LocationActors;

namespace Entities.Factories
{
    public class Factory : ReceiveActor
    {
        private FactoryState _factoryState;

        public Factory(string name, FactoryType factoryType, CelestialBody body, InventoryType inventoryType)
        {
            var inventory = new Inventory(inventoryType);
            _factoryState = new FactoryState(body, name, factoryType, inventory, 0);

            Receive<QueryState>(msg =>
            {
                Sender.Tell(_factoryState);
            });

            Receive<HeartBeatActor.FactoryTick>(msg =>
            {
                Context.LogMessageDebug("Tick received");
                var areInputRequirementsMet = AreInputRequirementsMet();
                if (areInputRequirementsMet)
                {
                    PerformTick();
                }
            });

            Receive<DepositResources>(msg =>
            {
                var stateInventory = _factoryState.Inventory.Resources;
                var newItems = new Dictionary<IResource,int>();
                foreach (var resource in msg.Resources.Keys)
                {
                    if (stateInventory.ContainsKey(resource))
                    {
                        newItems.Add(resource, stateInventory[resource] + msg.Resources[resource]);
                    }
                    else
                    {
                        newItems.Add(resource, msg.Resources[resource]);
                    }
                }

                var newInventory = stateInventory.AddRange(newItems);
                _factoryState = new FactoryState(_factoryState.Body, _factoryState.Name, _factoryState.FactoryType, new Inventory(_factoryState.Inventory.InventoryType, newInventory), _factoryState.TicksSinceLastUpdate);
            });
        }

        private void PerformTick()
        {
            var inv = ImmutableDictionary.CreateBuilder<IResource, int>();

            foreach (var quantityPeriod in _factoryState.FactoryType.InputResources)
            {
                var resource = quantityPeriod.Key;

                var quant = _factoryState.Inventory.Resources[resource];
                var newQuant = quant - quantityPeriod.Value.Quantity;
                inv.Add(resource, newQuant);
            }

            foreach (var outputResource in _factoryState.FactoryType.OutputResources)
            {
                var resource = outputResource.Key;

                int quant = 0;
                if (_factoryState.Inventory.Resources.ContainsKey(resource))
                {
                    quant = _factoryState.Inventory.Resources[resource];
                }

                var newQuant = quant + outputResource.Value.Quantity;
                inv.Add(resource, newQuant);
            }

            // is this really necessary? why would a factory contain/accept anything that isnt in its input or output requirements?
            //foreach (var keyValuePair in _factoryState.Inventory.Resources)
            //{
            //    if (!inv.ContainsKey(keyValuePair.Key))
            //    {
            //        inv.Add(keyValuePair.Key, keyValuePair.Value);
            //    }
            //}

            var inventory = new Inventory(_factoryState.Inventory.InventoryType, inv.ToImmutable());
            _factoryState = new FactoryState(_factoryState.Body, _factoryState.Name, _factoryState.FactoryType, inventory, _factoryState.TicksSinceLastUpdate);
        }

        /// <summary>
        /// Establishes if it is time for tick, then if resources are met.
        /// </summary>
        /// <remarks>This has a weakness of if ticks have passed but resources not met then when resources are met will immediatley produce.
        /// It also fails in that if the  imput requirements are met then the output starts to progress.</remarks>
        /// <returns>True if sufficient period has passed to update and resources are available</returns>
        private bool AreInputRequirementsMet()
        {
            bool retVal = true;
            var noTicks = _factoryState.TicksSinceLastUpdate + 1;
            if (noTicks >= _factoryState.GetMinTicksPerUpdate())
            {
                foreach (var resource in _factoryState.FactoryType.InputResources.Keys)
                {
                    // if inventory doesnt contain an input resource then requirements not met
                    if (!_factoryState.Inventory.Resources.ContainsKey(resource))
                    {
                        retVal = false;
                        break;
                    }

                    var requiredAmount = _factoryState.FactoryType.InputResources[resource].Quantity;
                    var inventoryAmount = _factoryState.Inventory.Resources[resource];

                    if (inventoryAmount < requiredAmount)
                    {
                        retVal = false;
                        break;
                    }
                }
            }
            else
            {
                retVal = false;
            }

            return retVal;
        }

        public static Props CreateProps(string name, FactoryType factoryType, CelestialBody body, InventoryType inventoryType)
        {
            return Props.Create(() => new Factory(name, factoryType, body, inventoryType));
        }

        /// <summary>
        /// The immutable factory state
        /// </summary>
        public class FactoryState
        {
            /// <summary>
            /// The <see cref="CelestialBody"/> the factory is attached to
            /// </summary>
            public CelestialBody Body { get; private set; }
            public string Name { get; private set; }

            /// <summary>
            /// The resource generation / consumption of the factory.
            /// </summary>
            public FactoryType FactoryType { get; private set; }
            public Inventory Inventory { get; private set; }

            public int TicksSinceLastUpdate { get; private set; }

            /// <summary>
            /// Gets the largest period for input resources. This is minimum time per update.
            /// </summary>
            /// <returns>Number of ticks required for the inputs to be consumed.</returns>
            public int GetMinTicksPerUpdate()
            {
                int max = 0;
                if (FactoryType.InputResources.Any())
                {
                    max = FactoryType.InputResources.Values.Select(i => i.Periods).Max();
                }
                return max;
            }

            public FactoryState(CelestialBody body, string name, FactoryType factoryType, Inventory inventory, int ticksSinceLastUpdate)
            {
                Body = body;
                Name = name;
                FactoryType = factoryType;
                Inventory = inventory;
                TicksSinceLastUpdate = ticksSinceLastUpdate;
            }
        }

        public class QueryState
        {
        }

        public class DepositResources
        {
            public ImmutableDictionary<IResource, int> Resources { get; private set; }

            public DepositResources(ImmutableDictionary<IResource, int> resources)
            {
                Resources = resources;
            }
        }
    }
}