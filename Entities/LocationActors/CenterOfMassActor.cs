using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities.LocationActors
{
    /// <summary>
    /// Represents a center of mass of some form of solar system
    /// </summary>
    /// <remarks>
    /// Am considering modelling many of these as being part of clusters of larger systems 
    /// </remarks>
    public class CenterOfMassActor : ReceiveActor
    {
        public List<Star> Stars = new List<Star>();
        public List<Planet> Planets = new List<Planet>();
        public List<AsteroidField> AsteroidFields = new List<AsteroidField>();
     
    }

    /// <summary>
    /// Placeholder to remind thought
    /// </summary>
    public class AsteroidField
    {
        // how the fuck woudl this work? .... do we make this asteroids?
        // plan was to use these for mines.
        // anticipation was to regard planets as essentially multi mines.
    }

    /// <summary>
    /// Represents a star orbiting a COM
    /// </summary>
    public class Star : ICelestialBody
    {
        public Star(StarType details, CelestialBody bodyData)
        {
            Details = details;
            BodyData = bodyData;
        }

        /// <summary>
        /// Type data for the start
        /// </summary>
        public StarType Details { get; }

        /// <summary>
        /// THe body data for the celestial body.
        /// </summary>
        public CelestialBody BodyData { get; }

        /// <summary>
        /// Encapsulates data to be repeated for multiple Stars of different types
        /// </summary>
        public class StarType
        {
            public StarType(double fuelRate)
            {
                FuelRate = fuelRate;
            }

            /// <summary>
            /// Refuel rate from the star
            /// </summary>
            public double FuelRate { get; }
        }
    }
}
