using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Entities.Observation;
using NUnit.Framework.Constraints;

namespace Entities.LocationActors
{
    /// <summary>
    /// This is the actor that is responsible for managing and aggregating info abotu the Center of Mass actors
    /// </summary>
    /// <remarks>
    /// This is also the actor that will be responsible for essentially producing a map and so therefor all the high level cross COM stuff.
    /// Eg user at location and wants to see all connections 4 levels deep
    /// Eg a user at location wants to see nearby markets.
    /// </remarks>
    public class CenterOfMassManagerActor : ReceiveActor
    {
        private readonly IActorRef _factoryCoordinator;
        private readonly Dictionary<string,IActorRef> _centerOfMasses = new Dictionary<string, IActorRef>();
        private readonly List<IActorRef> _contentsChangedObservers = new List<IActorRef>();

        private readonly Dictionary<CelestialBody,IActorRef> _celestialBodiesOnCenterOfMasses = new Dictionary<CelestialBody, IActorRef>();
        

        public static string Name => "CenterOfMassManagerActor";

        public static string Path => @"user\CenterOfMassManagerActor";

        public static Props CreateProps(IActorRef factoryCoordinator)
        {
            return Props.Create(() => new CenterOfMassManagerActor(factoryCoordinator));
        }

        public CenterOfMassManagerActor(IActorRef factoryCoordinator)
        {
            _factoryCoordinator = factoryCoordinator;
            Receive<CreateCenterOfMass>(msg =>
            {
                var com = Context.ActorOf(CenterOfMassActor.CreateProps(msg.Name, msg.Stars, msg.Planets, _factoryCoordinator), msg.Name.RemoveSpaces());
                _centerOfMasses.Add(msg.Name, com);

                foreach (var star in msg.Stars)
                {
                    _celestialBodiesOnCenterOfMasses.Add(star, com);
                }

                foreach (var planet in msg.Planets)
                {
                    _celestialBodiesOnCenterOfMasses.Add(planet, com);
                }

                foreach (var contentsChangedObserver in _contentsChangedObservers)
                {
                    contentsChangedObserver.Tell(new EventObserved());
                }
            });

            Receive<QueryCenterOfMasses>(msg =>
            {
                if (string.IsNullOrWhiteSpace(msg.NameToQuery))
                {
                    var dic = _centerOfMasses.ToDictionary(centerOfMass => centerOfMass.Key, centerOfMass => centerOfMass.Value);
                    Sender.Tell(new CenterOfMassQueryResult(dic));
                }
                else
                {
                    if (_centerOfMasses.ContainsKey(msg.NameToQuery))
                    {
                        var dic = new Dictionary<string, IActorRef> {{msg.NameToQuery, _centerOfMasses[msg.NameToQuery]}};
                        Sender.Tell(new CenterOfMassQueryResult(dic));
                    }
                    else
                    {
                        Sender.Tell(new CenterOfMassQueryResult(new Dictionary<string, IActorRef>()));
                    }
                }
            });

            Receive<UpdateDelta>(msg =>
            {
                foreach (var actorRef in _centerOfMasses.Values)
                {
                    actorRef.Tell(msg);
                }
            });

            Receive<Observe>(msg =>
            {
                _contentsChangedObservers.Add(Sender);
            });

            Receive<UnObserve>(msg =>
            {
                _contentsChangedObservers.Remove(Sender);
            });
        }

        public class GetCenterOfMass
        {
        }

        public class CenterOfMassQueryResult
        {
            public Dictionary<string, IActorRef> CenterOfMasses { get; }

            public CenterOfMassQueryResult(Dictionary<string, IActorRef> centerOfMasses)
            {
                CenterOfMasses = centerOfMasses;
            }
        }

        public class QueryCenterOfMasses
        {
            public QueryCenterOfMasses(string nameToQuery)
            {
                NameToQuery = nameToQuery;
            }

            public string NameToQuery { get;  }
        }

        public class CreateCenterOfMass
        {
            public CreateCenterOfMass(string name, CelestialBody[] stars, CelestialBody[] planets)
            {
                Stars = stars;
                Planets = planets;
                Name = name;
            }

            public CelestialBody[] Stars { get; private set; }
            public CelestialBody[] Planets { get; private set; }
            public string Name { get; private set; }
        }


    }
}
