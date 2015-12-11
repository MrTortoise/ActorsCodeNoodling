using Akka.Actor;
using Entities.LocationActors;

namespace Entities.Factories
{
    public class Factory : ReceiveActor
    {
        private readonly FactoryState _factoryState;

        public Factory(string name, FactoryType factoryType, CelestialBody body)
        {
            _factoryState = new FactoryState(body, name, factoryType);

            Receive<QueryState>(msg =>
            {
                Sender.Tell(_factoryState);
            });
        }

        public static Props CreateProps(string name, FactoryType factoryType, CelestialBody body)
        {
            return Props.Create(() => new Factory(name, factoryType, body));
        }

        public class FactoryState
        {
            public CelestialBody Body { get; private set; }
            public string Name { get; private set; }
            public FactoryType FactoryType { get; private set; }

            public FactoryState(CelestialBody body, string name, FactoryType factoryType)
            {
                Body = body;
                Name = name;
                FactoryType = factoryType;
            }
        }

        public class QueryState
        {
        }
    }
}