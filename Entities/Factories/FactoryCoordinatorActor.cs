using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Entities.LocationActors;

namespace Entities.Factories
{
    public class FactoryCoordinatorActor : ReceiveActor
    {
        public const string Name = "FactoryCoordinatorActor";

        private ImmutableHashSet<IActorRef> _factories = ImmutableHashSet<IActorRef>.Empty;
        private ImmutableHashSet<FactoryType> _factoryTypes = ImmutableHashSet<FactoryType>.Empty;

        public static Props CreateProps()
        {
            return Props.Create(() => new FactoryCoordinatorActor());
        }

        public FactoryCoordinatorActor()
        {
            Receive<FactoryType>(msg =>
            {
                Context.LogMessageDebug(msg);
               _factoryTypes =  _factoryTypes.Add(msg);
            });

            Receive<QueryFactoryTypes>(msg =>
            {
                Context.LogMessageDebug(msg);
                Sender.Tell(new FactoryTypesResult(_factoryTypes.ToArray()));
            });

            Receive<CreateFactory>(msg =>
            {
                Context.LogMessageDebug(msg);
                var factory = Context.ActorOf(Factory.CreateProps(msg.Name, msg.FactoryType, msg.Body, msg.InventoryType));
                _factories = _factories.Add(factory);
                Sender.Tell(new FactoryCreated(factory, Sender, msg.Body));
                msg.Owner.Tell(new FactoryCreated(factory,Sender,msg.Body));
            });

            Receive<QueryFactories>(msg =>
            {
                Context.LogMessageDebug(msg);
                Sender.Tell(new FactoryQueryResult(_factories));
            });
        }

        public class FactoryCreated
        {
            public IActorRef Factory { get; private set; }
            public IActorRef CenterOfMass { get; private set; }
            public CelestialBody CelestialBody { get; private set; }

            public FactoryCreated(IActorRef factory, IActorRef centerOfMass, CelestialBody body)
            {
                if (factory == null) throw new ArgumentNullException(nameof(factory));
                if (centerOfMass == null) throw new ArgumentNullException(nameof(centerOfMass));
                if (body == null) throw new ArgumentNullException(nameof(body));

                Factory = factory;
                CenterOfMass = centerOfMass;
                CelestialBody = body;
            }
        }

        public class FactoryTypesResult
        {
            public FactoryTypesResult(FactoryType[] factoryTypes)
            {
                if (factoryTypes == null) throw new ArgumentNullException(nameof(factoryTypes));

                FactoryTypes = factoryTypes;
            }

            public FactoryType[] FactoryTypes { get; private set; }
        }

        public class QueryFactoryTypes
        {
        }

        public class CreateFactory
        {
            public string Name { get; private set; }
            public FactoryType FactoryType { get; private set; }
            public IActorRef Owner { get; private set; }
            public CelestialBody Body { get; private set; }
            public InventoryType InventoryType { get; private set; }

            public CreateFactory(string name, FactoryType factoryType, IActorRef owner, CelestialBody body, InventoryType inventoryType)
            {
                if (factoryType == null) throw new ArgumentNullException(nameof(factoryType));
                if (owner == null) throw new ArgumentNullException(nameof(owner));
                if (body == null) throw new ArgumentNullException(nameof(body));
                if (inventoryType == null) throw new ArgumentNullException(nameof(inventoryType));
                if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is null or whitespace", nameof(name));

                Name = name;
                FactoryType = factoryType;
                Owner = owner;
                Body = body;
                InventoryType = inventoryType;
            }
        }

        public class QueryFactories
        {
        }

        public class FactoryQueryResult
        {
            public ImmutableHashSet<IActorRef> Factories { get;private set; }

            public FactoryQueryResult(ImmutableHashSet<IActorRef> factories)
            {
                if (factories == null) throw new ArgumentNullException(nameof(factories));

                Factories = factories;
            }
        }
    }
}