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
               _factoryTypes =  _factoryTypes.Add(msg);
            });

            Receive<QueryFactoryTypes>(msg =>
            {
                Sender.Tell(new FactoryTypesResult(_factoryTypes.ToArray()));
            });

            Receive<CreateFactory>(msg =>
            {
                var factory = Context.ActorOf(Factory.CreateProps(msg.Name, msg.FactoryType, msg.Body));
                _factories = _factories.Add(factory);
                Sender.Tell(new FactoryCreated(factory, Sender, msg.Body));
                msg.Owner.Tell(new FactoryCreated(factory,Sender,msg.Body));
            });

            Receive<QueryFactories>(msg =>
            {
                Sender.Tell(new FactoryQueryResult(_factories));
            });
        }

        public class FactoryCreated
        {
            public IActorRef Factory { get; private set; }
            public IActorRef CenterOfMass { get; private set; }
            public CelestialBody CelestialBody { get; set; }

            public FactoryCreated(IActorRef factory, IActorRef centerOfMass, CelestialBody body) 
            {
                Factory = factory;
                CenterOfMass = centerOfMass;
                CelestialBody = body;
            }
        }

        public class FactoryTypesResult
        {
            public FactoryTypesResult(FactoryType[] factoryTypes)
            {
                FactoryTypes = factoryTypes;
            }

            public FactoryType[] FactoryTypes { get; private set; }
        }

        public class QueryFactoryTypes
        {
        }

        public class CreateFactory
        {
            public string Name { get; set; }
            public FactoryType FactoryType { get; set; }
            public IActorRef Owner { get; set; }
            public CelestialBody Body { get; set; }

            public CreateFactory(string name, FactoryType factoryType, IActorRef owner, CelestialBody body)
            {
                Name = name;
                FactoryType = factoryType;
                Owner = owner;
                Body = body;
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
                Factories = factories;
            }
        }
    }
}