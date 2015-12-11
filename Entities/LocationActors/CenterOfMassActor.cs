using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Akka.Actor;
using Entities.Factories;

namespace Entities.LocationActors
{
    /// <summary>
    /// Represents a center of mass of some form of solar system
    /// </summary>
    /// <remarks>
    /// Am considering modelling many of these as being part of clusters of larger systems.
    /// _stars dotn need to be actors ... but what about things that have things like markets?
    /// </remarks>
    public class CenterOfMassActor : ReceiveActor
    {
        private readonly IActorRef _factoryCoordinator;
        private CenterOfMassState _centerOfMassState;

        public static Props CreateProps(string name, CelestialBody[] stars, CelestialBody[] planets, IActorRef factoryCoordinator, ImmutableDictionary<IActorRef, CelestialBody> factories = null)
        {
            factories = factories ?? ImmutableDictionary<IActorRef, CelestialBody>.Empty;
            return Props.Create(() => new CenterOfMassActor(name, stars, planets, factoryCoordinator, factories));
        }

        public CenterOfMassActor(string name, CelestialBody[] stars, CelestialBody[] planets, IActorRef factoryCoordinator, ImmutableDictionary<IActorRef, CelestialBody> factories) 
        {
            if (stars == null) throw new ArgumentNullException(nameof(stars));
            if (planets == null) throw new ArgumentNullException(nameof(planets));
            if (factoryCoordinator == null) throw new ArgumentNullException(nameof(factoryCoordinator));
            if (factories == null) throw new ArgumentNullException(nameof(factories));

            _factoryCoordinator = factoryCoordinator;
            _centerOfMassState = new CenterOfMassState(name, stars, planets, factories);
            Receive<UpdateDelta>(msg =>
            {
                Context.LogMessageDebug(msg);
                foreach (var star in _centerOfMassState.Stars)
                {
                    star.UpdateDelta(msg.Delta);
                }

                foreach (var planet in _centerOfMassState.Planets)
                {
                    planet.UpdateDelta(msg.Delta);
                }
            });

            Receive<CenterOfMassStateQuery>(msg =>
            {
                Context.LogMessageDebug(msg);
                Sender.Tell(new CenterOfMassQueryResult(_centerOfMassState.Stars, _centerOfMassState.Planets));
            });

            Receive<CreateFactoryOnBody>(msg =>
            {
                Context.LogMessageDebug(msg);
                Debug.Assert(_centerOfMassState.UnionCelestialBodies().Contains(msg.Body));
                _factoryCoordinator.Tell(new FactoryCoordinatorActor.CreateFactory(msg.Name, msg.FactoryType, Sender, msg.Body));
            });

            Receive<FactoryCoordinatorActor.FactoryCreated>(msg =>
            {
                Context.LogMessageDebug(msg);
                Debug.Assert(_centerOfMassState.UnionCelestialBodies().Contains(msg.CelestialBody));
                var state = new CenterOfMassState(_centerOfMassState.Name, _centerOfMassState.Stars, _centerOfMassState.Planets, _centerOfMassState.Factories.Add(msg.Factory, msg.CelestialBody));
                _centerOfMassState = state;
            });
        }

        public class CenterOfMassStateQuery
        {
        }

        public class CenterOfMassQueryResult
        {
            public CelestialBody[] Stars { get;  }
            public CelestialBody[] Planets { get;  }

            public CenterOfMassQueryResult(CelestialBody[] stars, CelestialBody[] planets)
            {
                Stars = stars;
                Planets = planets;
            }
        }

        public class CreateFactoryOnBody
        {
            public string Name { get; private set; }
            public FactoryType FactoryType { get; private set; }
            public CelestialBody Body { get; private set; }

            public CreateFactoryOnBody(string name, FactoryType factoryType, CelestialBody body)
            {
                Name = name;
                FactoryType = factoryType;
                Body = body;
            }
        }
    }
}
