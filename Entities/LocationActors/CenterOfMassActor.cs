﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
        private readonly CenterOfMassState _centerOfMassState;

        /// <summary>
        /// Gets the COM Name
        /// </summary>
        public string Name => _centerOfMassState.Name;

        /// <summary>
        /// Gets the stars in this COM
        /// </summary>
        public CelestialBody[] Stars => _centerOfMassState.Stars;

        /// <summary>
        /// Gets the planets and their bodies in this COM
        /// </summary>
        public CelestialBody[] Planets => _centerOfMassState.Planets;

        public static Props CreateProps(string name, CelestialBody[] stars, CelestialBody[] planets,IActorRef factoryCoordinator, Dictionary<CelestialBody, IActorRef> factories = null)
        {
            factories = factories ?? new Dictionary<CelestialBody, IActorRef>();
            return Props.Create(() => new CenterOfMassActor(name, stars, planets, factoryCoordinator, factories));
        }

        public CenterOfMassActor(string name, CelestialBody[] stars, CelestialBody[] planets, IActorRef factoryCoordinator, Dictionary<CelestialBody, IActorRef> factories) 
        {
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
                Sender.Tell(new CenterOfMassQueryResult(Stars, Planets));
            });

            Receive<CreateFactoryOnBody>(msg =>
            {
                Context.LogMessageDebug(msg);
                _factoryCoordinator.Tell(new FactoryCoordinatorActor.CreateFactory(msg.Name, msg.FactoryType, Sender,
                    msg.Body));
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
