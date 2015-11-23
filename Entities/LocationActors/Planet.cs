using System.Collections.Generic;

namespace Entities.LocationActors
{
    /// <summary>
    /// A planet orbiting around the COM of a system
    /// </summary>
    public class Planet : ICelestialBody
    {
        public Planet(PlanetType details, CelestialBody bodyData)
        {
            Details = details;
            BodyData = bodyData;
        }

        /// <summary>
        /// The composition details of the planet
        /// </summary>
        public PlanetType Details { get; }

        /// <summary>
        /// The data for the body component.
        /// </summary>
        public CelestialBody BodyData { get; }

        /// <summary>
        /// Represents a compositional configuration of a planet.
        /// </summary>
        public class PlanetType
        {
            public PlanetType(Dictionary<Resource, double> materialComposition)
            {
                MaterialComposition = materialComposition;
            }

            /// <summary>
            /// The material composition of the planet.
            /// </summary>
            public Dictionary<Resource,double> MaterialComposition { get; }
        }
    }
}