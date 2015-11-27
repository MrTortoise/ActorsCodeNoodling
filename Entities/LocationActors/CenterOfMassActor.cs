using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

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
        private readonly CenterOfMassState _centerOfMassState;

        /// <summary>
        /// Gets the COM Name
        /// </summary>
        public string Name => _centerOfMassState.Name;

        /// <summary>
        /// Gets the stars in this COM
        /// </summary>
        public Star[] Stars => _centerOfMassState.Stars;

        /// <summary>
        /// Gets the planets and their bodies in this COM
        /// </summary>
        public Planet[] Planets => _centerOfMassState.Planets;

        public static Props CreateProps(string name, Star[] stars, Planet[] planets)
        {
            return Props.Create(() => new CenterOfMassActor(name, stars, planets));
        }

        public CenterOfMassActor(string name, Star[] stars, Planet[] planets) 
        {
            _centerOfMassState = new CenterOfMassState(name, stars, planets);
            Receive<UpdateDelta>(msg =>
            {
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
                Sender.Tell(new CenterOfMassQueryResult(Stars, Planets));
            });
        }

        public class CenterOfMassStateQuery
        {
        }

        public class CenterOfMassQueryResult
        {
            public Star[] Stars { get;  }
            public Planet[] Planets { get;  }

            public CenterOfMassQueryResult(Star[] stars, Planet[] planets)
            {
                Stars = stars;
                Planets = planets;
            }
        }
    }
}
