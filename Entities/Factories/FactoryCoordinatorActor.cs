using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Entities.LocationActors;

namespace Entities.Factories
{
    public class FactoryCoordinatorActor : ReceiveActor
    {
        private readonly HashSet<IActorRef> _factories = new HashSet<IActorRef>();
        private readonly HashSet<FactoryType> _factoryTypes = new HashSet<FactoryType>();
        public const string Name = "FactoryCoordinatorActor";

        public static Props CreateProps()
        {
            return Props.Create(() => new FactoryCoordinatorActor());
        }

        public FactoryCoordinatorActor()
        {
            Receive<RegisterFactory>(msg =>
            {
                _factories.Add(msg.Factory);
            });

            Receive<FactoryType>(msg =>
            {
                _factoryTypes.Add(msg);
            });

            Receive<QueryFactoryTypes>(msg =>
            {
                Sender.Tell(new FactoryTypesResult(_factoryTypes.ToArray()));
            });

            Receive<CreateFactory>(msg =>
            {

            });
        }


        public class RegisterFactory
        {
            public IActorRef Factory { get; }

            public RegisterFactory(IActorRef factory)
            {
                Factory = factory;
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
    }
}