using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Entities.Factories;
using Entities.LocationActors;

namespace Entities
{
    /// <summary>
    /// This is an actor representing a trader. 
    /// </summary>
    public class Trader : ReceiveActor, ITrader
    {
       // private readonly IActorRef _factoryCoordinatorActor;
        private readonly Dictionary<IResource, ResourceStack> _resources;
        private ImmutableHashSet<IActorRef> _factories = ImmutableHashSet<IActorRef>.Empty;

        /// <summary>
        /// Creates a Trader with a name
        /// </summary>
        /// <param name="name"></param>
        public Trader(string name)
        {
           // _factoryCoordinatorActor = factoryCoordinatorActor;
            _resources = new Dictionary<IResource, ResourceStack>();

            Name = name;

            Receive<PostResourceMessage>(message =>
            {
                Context.LogMessageDebug(message);
                if (_resources.ContainsKey(message.ResourceStack.Resource))
                {
                    var resourceStack = _resources[message.ResourceStack.Resource];
                    _resources[message.ResourceStack.Resource] = new ResourceStack(resourceStack.Resource,
                        resourceStack.Quantity + message.ResourceStack.Quantity);
                }
                else
                {
                    _resources.Add(message.ResourceStack.Resource, message.ResourceStack);
                }
            });

            Receive<QueryResourcesMessage>(message =>
            {
                Context.LogMessageDebug(message);
                Sender.Tell(new QueryResourcesResultMessage(_resources.Values.ToImmutableArray()));
            });

            Receive<CreateFactoryOnBody>(msg =>
            {
                msg.CenterOfMassActor.Tell(new CenterOfMassActor.CreateFactoryOnBody(msg.Name, msg.FactoryType, msg.Body, msg.InventoryType));
            });

            Receive<FactoryCoordinatorActor.FactoryCreated>(msg =>
            {
                Context.LogMessageDebug(msg);
                _factories = _factories.Add(msg.Factory);
            });

            Receive<QueryFactories>(msg =>
            {
                Sender.Tell(new FactoryQueryResult(_factories));
            });
        }

        public class CreateFactoryOnBody
        {
            public CreateFactoryOnBody(IActorRef centerOfMassActor, string name, FactoryType factoryType, CelestialBody body, InventoryType inventoryType)
            {
                if (centerOfMassActor == null) throw new ArgumentNullException(nameof(centerOfMassActor));
                if (factoryType == null) throw new ArgumentNullException(nameof(factoryType));
                if (body == null) throw new ArgumentNullException(nameof(body));
                if (inventoryType == null) throw new ArgumentNullException(nameof(inventoryType));
                if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is null or whitespace", nameof(name));

                CenterOfMassActor = centerOfMassActor;
                Name = name;
                FactoryType = factoryType;
                Body = body;
                InventoryType = inventoryType;
            }

            public IActorRef CenterOfMassActor { get; private set; }
            public string Name { get; private set; }
            public FactoryType FactoryType { get; private set; }
            public CelestialBody Body { get; private set; }
            public InventoryType InventoryType { get; private set; }
        }

        public string Name { get; }

        public struct PostResourceMessage
        {
            public ResourceStack ResourceStack { get; }

            public PostResourceMessage(ResourceStack resourceStack)
            {
                ResourceStack = resourceStack;
            }
        }

        public struct QueryResourcesMessage
        {
        }

        public struct QueryResourcesResultMessage
        {
            public QueryResourcesResultMessage(ImmutableArray<ResourceStack> resourceStacks)
            {
                ResourceStacks = resourceStacks;
            }

            public ImmutableArray<ResourceStack> ResourceStacks { get; }
        }

        public class FactoryQueryResult
        {
            public ImmutableHashSet<IActorRef> Factories { get; private set; }


            public FactoryQueryResult(ImmutableHashSet<IActorRef> factories)
            {
                Factories = factories;
            }
        }

        public class QueryFactories
        {
        }
    }
}